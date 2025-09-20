using ComputerSales.Domain.Entity.EVariant;
using ComputerSales.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace ComputerSalesProject_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var now = DateTime.UtcNow;

            var products = _context.Products
                .Select(p => new ProductCardVM
                {
                    ProductId = (int)p.ProductID,
                    SKU = p.SKU,
                    // Hiển thị tên: dùng ShortDescription cho “tên sản phẩm” dễ đọc
                    Name = p.ShortDescription,
                    ShortDescription = p.ShortDescription,

                    // Ảnh đại diện: lấy 1 ảnh sort nhỏ nhất trong tất cả variants
                    Images = p.ProductVariants
                             .SelectMany(v => v.VariantImages
                                 .OrderBy(vi => vi.SortOrder)
                                 .Select(vi => vi.Url))
                             .Take(1)
                             .ToList(),

                    // Giá cấp product: chọn dòng giá đang hiệu lực “mới nhất” trên mọi variant
                    Price = p.ProductVariants
                             .SelectMany(v => v.VariantPrices
                                 .Where(vp => vp.Status == PriceStatus.Active
                                           && (vp.ValidFrom == null || vp.ValidFrom <= now)
                                           && (vp.ValidTo == null || vp.ValidTo >= now))
                                 .OrderByDescending(vp => vp.ValidFrom ?? DateTime.MinValue)
                                 .Select(vp => vp.Price))
                             .FirstOrDefault(),

                    DiscountPrice = p.ProductVariants
                             .SelectMany(v => v.VariantPrices
                                 .Where(vp => vp.Status == PriceStatus.Active
                                           && (vp.ValidFrom == null || vp.ValidFrom <= now)
                                           && (vp.ValidTo == null || vp.ValidTo >= now))
                                 .OrderByDescending(vp => vp.ValidFrom ?? DateTime.MinValue)
                                 .Select(vp => vp.DiscountPrice))
                             .FirstOrDefault(),

                    // Danh sách biến thể + giá/ảnh đang hiệu lực từng biến thể
                    Variants = p.ProductVariants
                        .Select(v => new VariantVM
                        {
                            VariantId = v.Id,
                            SKU = v.SKU,
                            Quantity = v.Quantity,

                            Price = v.VariantPrices
                                .Where(vp => vp.Status == PriceStatus.Active
                                          && (vp.ValidFrom == null || vp.ValidFrom <= now)
                                          && (vp.ValidTo == null || vp.ValidTo >= now))
                                .OrderByDescending(vp => vp.ValidFrom ?? DateTime.MinValue)
                                .Select(vp => vp.Price)
                                .FirstOrDefault(),

                            DiscountPrice = v.VariantPrices
                                .Where(vp => vp.Status == PriceStatus.Active
                                          && (vp.ValidFrom == null || vp.ValidFrom <= now)
                                          && (vp.ValidTo == null || vp.ValidTo >= now))
                                .OrderByDescending(vp => vp.ValidFrom ?? DateTime.MinValue)
                                .Select(vp => vp.DiscountPrice)
                                .FirstOrDefault(),

                            Images = v.VariantImages
                                .OrderBy(vi => vi.SortOrder)
                                .Select(vi => vi.Url)
                                .ToList()
                        })
                        .ToList()
                })
                .AsEnumerable() // về client để set SelectedVariantId dễ dàng
                .Select(vm =>
                {
                    if (vm.Variants?.Count == 1)
                        vm.SelectedVariantId = vm.Variants[0].VariantId;
                    return vm;
                })
                .ToList();

            return View(products);
        }
    }
}
