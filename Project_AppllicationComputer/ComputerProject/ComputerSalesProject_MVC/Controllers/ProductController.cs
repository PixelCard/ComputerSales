using ComputerSales.Application.UseCase.Product_UC;
using ComputerSales.Application.UseCaseDTO.Product_DTO;
using ComputerSales.Infrastructure.Persistence;
using ComputerSalesProject_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ProductController : Controller
{
    private readonly AppDbContext _context;
    private readonly CreateProduct_UC _createUC;
    public async Task<IActionResult> Details(int id)
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
                    Prices = v.VariantPrices
                        .Where(vp => (vp.ValidFrom == null || vp.ValidFrom <= now) &&
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
            .FirstOrDefaultAsync();
        // ---- chọn variant mặc định (lấy variant có status = 1 hoặc số lượng > 0)
        var selectedVariant = product.Variants
            .OrderByDescending(v => v.Status)
            .ThenByDescending(v => v.Quantity)
            .FirstOrDefault();

        if (selectedVariant == null) return NotFound();

        // ---- Map sang ViewModel
        var vm = new ProductDetailViewModel
        {
            ProductId = product.ProductID,
            SKU = product.SKU,
            Title = string.IsNullOrEmpty(product.ShortDescription) ? product.Slug : product.ShortDescription,
            ShortDescription = product.ShortDescription,
            Status = product.Status,
            IsDeleted = product.IsDeleted,
            AccessoriesID = product.AccessoriesID,
            ProviderID = product.ProviderID,

            VariantId = selectedVariant.Id,
            VariantSku = selectedVariant.SKU,
            Quantity = selectedVariant.Quantity,
            Currency = selectedVariant.Prices.FirstOrDefault()?.Currency ?? "$",
            Price = (selectedVariant.Prices.FirstOrDefault()?.DiscountPrice > 0
                        ? selectedVariant.Prices.First().DiscountPrice!
                        : selectedVariant.Prices.FirstOrDefault()?.Price) ?? 0m,
            OldPrice = (selectedVariant.Prices.FirstOrDefault()?.DiscountPrice > 0
                        ? selectedVariant.Prices.FirstOrDefault()?.Price
                        : null),
            CurrentPriceRaw = selectedVariant.Prices.Select(p => new PriceRowVM
            {
                Id = p.Id,
                Currency = p.Currency,
                Price = p.Price,
                DiscountPrice = p.DiscountPrice,
                Status = p.Status,
                ValidFrom = p.ValidFrom,
                ValidTo = p.ValidTo
            }).FirstOrDefault(),
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

        // ---- Option Groups (từ toàn bộ variants)
        vm.OptionGroups = product.Variants
            .SelectMany(v => v.Options)
            .GroupBy(o => o.OptionTypeName)
            .Select(g => new OptionGroupVM
            {
                Name = g.Key,
                Items = g.Select(o => new OptionItemVM
                {
                    Label = o.Value,
                    Selected = selectedVariant.Options.Any(sel => sel.OptionTypeName == g.Key && sel.Value == o.Value),
                    Disabled = false
                }).DistinctBy(x => x.Label).ToList()
            }).ToList();

        // ---- Variants summary
        vm.Variants = product.Variants.Select(v => new VariantSummaryVM
        {
            VariantId = v.Id,
            SKU = v.SKU,
            Quantity = v.Quantity,
            DisplayPrice = (v.Prices.FirstOrDefault()?.DiscountPrice > 0
                                ? v.Prices.First().DiscountPrice
                                : v.Prices.FirstOrDefault()?.Price)
        }).ToList();

        // ---- Overview blocks
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
            }).ToListAsync();

        return View(vm);
    }
    public ProductController(CreateProduct_UC createUC, AppDbContext context)
    {
        _createUC = createUC;
        _context = context;
    }


    [HttpGet]
    public IActionResult Create()
    {
        return View(new ProductDTOInput("", 1, 0, 0, "", ""));

    }


    // POST: /Products/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductDTOInput input, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            // luôn trả lại đúng kiểu model nếu có lỗi validate
            return View(input);
        }

        try
        {
            var result = await _createUC.HandleAsync(input, ct);
            TempData["SuccessMessage"] = $"Đã tạo sản phẩm {result.SKU} thành công!";
            return RedirectToAction(nameof(Create)); // hoặc Index/Details
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(input);
        }
    }



}
