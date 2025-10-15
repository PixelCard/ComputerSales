using ComputerSales.Application.UseCase.Product_UC;
using ComputerSales.Application.UseCase.ProductVariant_UC;
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
        private readonly GetByIdProductVariant_UC getByIdProductVariant_UC;

        public ProductController(
            CreateProduct_UC createUC, 
            AppDbContext context, 
            GetByIdProductVariant_UC getByIdProductVariant_UC)
        {
            _createUC = createUC;
            _context = context;
            this.getByIdProductVariant_UC= getByIdProductVariant_UC;
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