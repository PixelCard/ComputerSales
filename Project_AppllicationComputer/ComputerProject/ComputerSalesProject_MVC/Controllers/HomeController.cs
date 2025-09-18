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
            var products = _context.Products
                .Select(p => new ProductCardVM
                {
                    ProductId = (int)p.ProductID,
                    SKU = p.SKU,
                    Name = p.Slug,
                    ShortDescription = p.ShortDescription,

                    Price = p.ProductVariants
                             .SelectMany(v => v.VariantPrices)
                             .Select(vp => vp.Price)
                             .FirstOrDefault(),

                    DiscountPrice = p.ProductVariants
                             .SelectMany(v => v.VariantPrices)
                             .Select(vp => vp.DiscountPrice)
                             .FirstOrDefault(),

                    Images = p.ProductVariants
                             .SelectMany(v => v.VariantImages)
                             .Select(img => img.Url)
                             .ToList(),

                    Variants = p.ProductVariants
                             .Select(v => new VariantVM
                             {
                                 VariantId = v.Id,
                                 SKU = v.SKU,
                                 Quantity = v.Quantity,
                                 Price = v.VariantPrices
                                         .Select(vp => vp.Price)
                                         .FirstOrDefault(),
                                 DiscountPrice = v.VariantPrices
                                         .Select(vp => vp.DiscountPrice)
                                         .FirstOrDefault(),
                                 Images = v.VariantImages
                                         .Select(vi => vi.Url)
                                         .ToList()
                             }).ToList()
                })
                .ToList();

            return View(products);
        }


    }
}
