using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ComputerSales.Application.UseCase.Product_UC;
using ComputerSales.Application.UseCaseDTO.Product_DTO;
using ComputerSales.Domain.Entity.EVariant;
using ComputerSales.Infrastructure.Persistence;
using ComputerSalesProject_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComputerSalesProject_MVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly CreateProduct_UC _createUC;

        public ProductController(CreateProduct_UC createUC, AppDbContext context)
        {
            _createUC = createUC;
            _context = context;
        }

        // ======================== PRODUCT DETAILS ======================== //
        [HttpGet("/product/details/{id:int}")]
        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var now = DateTime.UtcNow;

            var product = await _context.Products
                .AsNoTracking()
                .Where(p => p.ProductID == id)
                .Select(p => new
                {
                    p.ProductID,
                    p.SKU,
                    p.Slug,
                    p.ShortDescription,
                    p.Status,
                    p.IsDeleted,
                    p.AccessoriesID,
                    p.ProviderID,

                    Variants = p.ProductVariants.Select(v => new
                    {
                        v.Id,
                        v.SKU,
                        v.Quantity,
                        v.Status,

                        // chỉ lấy các hàng giá còn hiệu lực & Active
                        Prices = v.VariantPrices
                            .Where(vp =>
                                vp.Status == PriceStatus.Active &&
                                (vp.ValidFrom == null || vp.ValidFrom <= now) &&
                                (vp.ValidTo == null || vp.ValidTo >= now))
                            .OrderByDescending(vp => vp.ValidFrom)
                            .ToList(),

                        Images = v.VariantImages
                            .OrderBy(vi => vi.SortOrder)
                            .Select(vi => vi.Url)
                            .ToList(),

                        Options = v.VariantOptionValues.Select(vo => new
                        {
                            OptionTypeName = vo.OptionalValue.OptionType.Name,
                            Value = vo.OptionalValue.Value
                        }).ToList()
                    }).ToList()
                })
                .FirstOrDefaultAsync(ct);

            if (product == null) return NotFound();
            if (product.Variants == null || product.Variants.Count == 0) return NotFound();

            // Ưu tiên biến thể có giá còn hiệu lực, rồi tới Status, rồi Quantity
            var selectedVariant = product.Variants
                .OrderByDescending(v => v.Prices.Any())
                .ThenByDescending(v => v.Status)
                .ThenByDescending(v => v.Quantity)
                .FirstOrDefault();

            if (selectedVariant == null) return NotFound();

            // Hàng giá đầu tiên (mới nhất) sau khi đã lọc còn hiệu lực
            var firstPrice = selectedVariant.Prices.FirstOrDefault();
            var (displayPrice, displayOld, currency) = ResolveDisplayPrice(firstPrice, now);

            var vm = new ProductDetailViewModel
            {
                ProductId = product.ProductID,
                SKU = product.SKU,
                Title = string.IsNullOrWhiteSpace(product.ShortDescription) ? product.Slug : product.ShortDescription,
                ShortDescription = product.ShortDescription,
                Status = product.Status,
                IsDeleted = product.IsDeleted,
                AccessoriesID = product.AccessoriesID,
                ProviderID = product.ProviderID,

                VariantId = selectedVariant.Id,
                VariantSku = selectedVariant.SKU,
                Quantity = selectedVariant.Quantity,

                Currency = currency,
                Price = displayPrice,
                OldPrice = displayOld,
                CurrentPriceRaw = firstPrice == null ? null : new PriceRowVM
                {
                    Id = firstPrice.Id,
                    Currency = firstPrice.Currency,
                    Price = firstPrice.Price,
                    DiscountPrice = firstPrice.DiscountPrice,
                    Status = firstPrice.Status,
                    ValidFrom = firstPrice.ValidFrom,
                    ValidTo = firstPrice.ValidTo
                },
                PriceHistory = selectedVariant.Prices.Select(p => new PriceRowVM
                {
                    Id = p.Id,
                    Currency = p.Currency,
                    Price = p.Price,
                    DiscountPrice = p.DiscountPrice,
                    Status = p.Status,
                    ValidFrom = p.ValidFrom,
                    ValidTo = p.ValidTo
                }).ToList(),
                Images = selectedVariant.Images
            };

            // Option groups toàn bộ product - LẤY TẤT CẢ OptionalValue của OptionType assigned cho Product
            // First, get OptionTypes for the product
            var productOptionTypes = await _context.productOptionTypes
                .AsNoTracking()
                .Where(pot => pot.ProductId == product.ProductID)
                .Select(pot => new { pot.OptionTypeId, pot.OptionType.Name })
                .ToListAsync(ct);

            var optionTypeIds = productOptionTypes.Select(t => t.OptionTypeId).ToList();

            var allOptionalValues = await _context.optionalValues
                .AsNoTracking()
                .Where(ov => optionTypeIds.Contains(ov.OptionTypeId))
                .Select(ov => new { ov.OptionTypeId, ov.Value, ov.Id, ov.SortOrder, ov.Price })
                .OrderBy(ov => ov.SortOrder)
                .ToListAsync(ct);

            // Then group by OptionTypeId
            var optionalValuesByType = allOptionalValues.GroupBy(ov => ov.OptionTypeId).ToDictionary(g => g.Key, g => g.ToList());

            vm.OptionGroups = productOptionTypes.Select(t => new OptionGroupVM
            {
                Name = t.Name,
                Items = optionalValuesByType.TryGetValue(t.OptionTypeId, out var values) ? values.Select(ov => new OptionItemVM
                {
                    Id = ov.Id,
                    Label = ov.Value,
                    Price = ov.Price,
                    Selected = selectedVariant.Options.Any(sel => sel.OptionTypeName == t.Name && sel.Value == ov.Value),
                    Disabled = !product.Variants.Any(v => v.Options.Any(o => o.OptionTypeName == t.Name && o.Value == ov.Value))
                }).ToList() : new List<OptionItemVM>()
            }).ToList();

            // Variants summary (giá cho từng variant)
            vm.Variants = product.Variants.Select(v =>
            {
                var vp = v.Prices.FirstOrDefault(); // đã là giá còn hiệu lực
                var (price, old, _) = ResolveDisplayPrice(vp, now);
                return new VariantSummaryVM
                {
                    VariantId = v.Id,
                    SKU = v.SKU,
                    Quantity = v.Quantity,
                    Price = price,
                    OldPrice = old
                };
            }).ToList();

            // Overview blocks (nếu có)
            vm.OverviewBlocks = await _context.productOverviews
                .AsNoTracking()
                .Where(o => o.ProductId == vm.ProductId)
                .OrderBy(o => o.DisplayOrder)
                .Select(o => new OverviewBlockVM
                {
                    ProductOverviewId = o.ProductOverviewId,
                    BlockType = (OverviewBlockType)o.BlockType,
                    TextContent = o.TextContent,
                    ImageUrl = o.ImageUrl,
                    Caption = o.Caption,
                    DisplayOrder = o.DisplayOrder,
                    CreateDate = o.CreateDate
                })
                .ToListAsync(ct);

            return View("Details", vm);
        }

        // ======================== CREATE (demo) ======================== //
        [HttpGet("/product/create")]
        public IActionResult Create()
        {
            // demo view; nếu bạn có view riêng thì dùng đúng view đó
            return View(new ProductDTOInput("", 1, 0, 0, "", ""));
        }

        [HttpPost("/product/create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductDTOInput input, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(input);
            try
            {
                var result = await _createUC.HandleAsync(input, ct);
                TempData["SuccessMessage"] = $"Đã tạo sản phẩm {result.SKU} thành công!";
                return RedirectToAction(nameof(Create));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(input);
            }
        }

        [HttpGet("/product/variant-data/{id:int}")]
        public async Task<IActionResult> VariantData(int id)
        {
            var now = DateTime.UtcNow;

            // 1) Lấy thông tin cơ bản của biến thể + 1 hàng giá đang hiệu lực (mới nhất)
            var v = await _context.productVariants
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new
                {
                    x.Id,
                    x.SKU,
                    x.Quantity,
                    x.ProductId,

                    // hàng giá mới nhất (nếu hợp lệ)
                    PriceRow = x.VariantPrices
                        .OrderByDescending(p => p.ValidFrom ?? DateTime.MinValue)
                        .FirstOrDefault(),

                    Images = x.VariantImages
                        .OrderBy(i => i.SortOrder)
                        .Select(i => i.Url)
                        .ToList(),

                    // các option/value của chính biến thể này (để biết value đang chọn)
                    Selected = x.VariantOptionValues.Select(vo => new
                    {
                        TypeName = vo.OptionalValue.OptionType.Name,
                        Value = vo.OptionalValue.Value
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (v == null) return NotFound();

            // 2) Lấy toàn bộ OptionType gán cho Product (qua bảng ProductOptionType)
            var typeList = await _context.productOptionTypes
                .AsNoTracking()
                .Where(pot => pot.ProductId == v.ProductId)
                .Select(pot => new { pot.OptionTypeId, pot.OptionType.Name })
                .ToListAsync();

            var optionTypeIds = typeList.Select(t => t.OptionTypeId).ToList();

            // 3) Lấy TẤT CẢ OptionalValue cho mỗi OptionType của Product (KHÔNG qua VariantOptionValue)
            var allValuesByType = await _context.optionalValues
                .AsNoTracking()
                .Where(ov => optionTypeIds.Contains(ov.OptionTypeId))
                .Select(ov => new { ov.OptionTypeId, ov.Value, ov.SortOrder, ov.Price })
                .OrderBy(x => x.SortOrder)
                .ToListAsync();

            var optionGroups = typeList.Select(t => new
            {
                Name = t.Name,
                Values = allValuesByType
                    .Where(val => val.OptionTypeId == t.OptionTypeId)
                    .OrderBy(val => val.SortOrder)
                    .Select(val => new { label = val.Value, price = val.Price })
                    .ToList()
            }).ToList();

            // 4) Tính giá hiển thị
            var (price, oldPrice, currency) = ResolveDisplayPrice(v.PriceRow, now);

            // 5) Map selected -> dictionary để client highlight
            var selectedDict = v.Selected
                .GroupBy(s => s.TypeName)
                .ToDictionary(g => g.Key, g => g.First().Value);

            // 6) Thêm info cho available (value nào có variant) - group by TypeName
            var availableValuesByType = await _context.variantOptionValues
                .AsNoTracking()
                .Where(vo => vo.Variant.ProductId == v.ProductId)
                .Select(vo => new { TypeName = vo.OptionalValue.OptionType.Name, Value = vo.OptionalValue.Value })
                .Distinct()
                .ToListAsync();

            return Json(new
            {
                variantId = v.Id,
                variantSku = v.SKU,
                quantity = v.Quantity,
                images = v.Images,

                currency,
                price,
                oldPrice,

                // *** toàn bộ OptionType + value của Product ***
                optionGroups,
                // *** giá trị đang chọn của biến thể ***
                selectedOptions = selectedDict,
                // *** available values per TypeName ***
                availableValues = availableValuesByType.GroupBy(x => x.TypeName).ToDictionary(g => g.Key, g => g.Select(vv => vv.Value).ToList())
            });
        }

        /// <summary>
        /// Quy tắc giá:
        /// - Hết hạn/Inactive: discount = 0 → dùng Price gốc
        /// - 0..100: % khuyến mãi (final = Price * (1 - %/100), old = Price)
        /// - >100: DiscountPrice là giá đã giảm (final = DiscountPrice, old = Price nếu Price > final)
        /// </summary>
        private static (decimal price, decimal? oldPrice, string currency)
            ResolveDisplayPrice(VariantPrice? row, DateTime now)
        {
            if (row == null) return (0m, null, "VND");

            bool expired =
                row.Status != PriceStatus.Active ||
                (row.ValidFrom.HasValue && row.ValidFrom.Value > now) ||
                (row.ValidTo.HasValue && row.ValidTo.Value < now);

            var currency = string.IsNullOrWhiteSpace(row.Currency) ? "VND" : row.Currency;

            if (expired || row.DiscountPrice <= 0m)
                return (row.Price, null, currency);

            if (row.DiscountPrice <= 100m)
            {
                var final = Math.Round(row.Price * (1m - (row.DiscountPrice / 100m)),
                                       2, MidpointRounding.AwayFromZero);
                return (final, row.Price, currency);
            }

            var price = row.DiscountPrice;                // discount là "giá sau giảm"
            var old = row.Price > price ? row.Price : (decimal?)null;
            return (price, old, currency);
        }

        // ======================== PRICING HELPERS ======================== //
        private static bool IsExpired(VariantPrice p, DateTime nowUtc)
        {
            if (p == null) return true;
            if (p.Status != PriceStatus.Active) return true;
            if (p.ValidFrom.HasValue && p.ValidFrom.Value > nowUtc) return true;
            if (p.ValidTo.HasValue && p.ValidTo.Value < nowUtc) return true;
            return false;
        }
    }
}