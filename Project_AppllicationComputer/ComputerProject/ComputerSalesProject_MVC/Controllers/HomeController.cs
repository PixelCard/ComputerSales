using ComputerSales.Domain.Entity.EProduct;
using ComputerSales.Domain.Entity.EVariant;
using ComputerSales.Infrastructure.Persistence;
using ComputerSalesProject_MVC.Models;
using ComputerSalesProject_MVC.Models.Product_ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IActionResult> Index()
        {
            var now = DateTime.UtcNow;
            // Lấy danh sách cho lưới dưới (Featured/Popular/…)
             var featured = await _context.Products
                .AsNoTracking()
                .OrderByDescending(p => p.ProductID)
                .Take(12)
                .Select(p => new ProductCardVM
                {
                    ProductId = (int)p.ProductID,
                    SKU = p.SKU,
                    Name = string.IsNullOrEmpty(p.ShortDescription) ? p.Slug : p.ShortDescription,
                    ShortDescription = p.ShortDescription ?? "",
                    Price = p.ProductVariants.SelectMany(v => v.VariantPrices)
                             .Where(vp => (vp.ValidFrom == null || vp.ValidFrom <= now) &&
                                          (vp.ValidTo == null || vp.ValidTo >= now))
                             .OrderByDescending(vp => vp.ValidFrom)
                             .Select(vp => vp.Price).FirstOrDefault(),
                    DiscountPrice = p.ProductVariants.SelectMany(v => v.VariantPrices)
                             .Where(vp => (vp.ValidFrom == null || vp.ValidFrom <= now) &&
                                          (vp.ValidTo == null || vp.ValidTo >= now) &&
                                          vp.DiscountPrice > 0)
                             .OrderByDescending(vp => vp.ValidFrom)
                             .Select(vp => vp.DiscountPrice).FirstOrDefault(),
                    Images = p.ProductVariants.SelectMany(v => v.VariantImages)
                             .OrderBy(i => i.SortOrder).Select(i => i.Url).Distinct().Take(4).ToList(),
                    Variants = p.ProductVariants.Select(v => new VariantVM
                    {
                        VariantId = v.Id,
                        SKU = v.SKU,
                        Quantity = v.Quantity,
                        Price = v.VariantPrices
                            .Where(vp => (vp.ValidFrom == null || vp.ValidFrom <= now) &&
                                         (vp.ValidTo == null || vp.ValidTo >= now))
                            .OrderByDescending(vp => vp.ValidFrom)
                            .Select(vp => (decimal?)vp.Price).FirstOrDefault(),
                        DiscountPrice = v.VariantPrices
                            .Where(vp => (vp.ValidFrom == null || vp.ValidFrom <= now) &&
                                         (vp.ValidTo == null || vp.ValidTo >= now) &&
                                         vp.DiscountPrice > 0)
                            .OrderByDescending(vp => vp.ValidFrom)
                            .Select(vp => (decimal?)vp.DiscountPrice).FirstOrDefault(),
                        Images = v.VariantImages.OrderBy(i => i.SortOrder).Select(i => i.Url).ToList()
                    }).ToList()
                })
                .ToListAsync();

            // Lấy danh sách “Sản phẩm mới” cho block trên
            var newArrivals = await _context.Products
                .AsNoTracking()
                .OrderByDescending(p => p.ProductID)
                .Take(12)
                .Select(p => new ProductListItemVM
                {
                    ProductId = (int)p.ProductID,
                    SKU = p.SKU,
                    Title = string.IsNullOrEmpty(p.ShortDescription) ? p.Slug : p.ShortDescription,
                    Slug = p.Slug,
                    ImageUrl = p.ProductVariants.SelectMany(v => v.VariantImages)
                               .OrderBy(i => i.SortOrder).Select(i => i.Url)
                               .FirstOrDefault() ?? "/images/placeholder.svg",
                    Currency = p.ProductVariants.SelectMany(v => v.VariantPrices)
                               .Where(vp => (vp.ValidFrom == null || vp.ValidFrom <= now) &&
                                            (vp.ValidTo == null || vp.ValidTo >= now))
                               .OrderByDescending(vp => vp.ValidFrom)
                               .Select(vp => vp.Currency).FirstOrDefault() ?? "$",
                    Price = p.ProductVariants.SelectMany(v => v.VariantPrices)
                               .Where(vp => (vp.ValidFrom == null || vp.ValidFrom <= now) &&
                                            (vp.ValidTo == null || vp.ValidTo >= now))
                               .OrderByDescending(vp => vp.ValidFrom)
                               .Select(vp => vp.DiscountPrice > 0 ? vp.DiscountPrice : vp.Price)
                               .FirstOrDefault(),
                    OldPrice = p.ProductVariants.SelectMany(v => v.VariantPrices)
                               .Where(vp => (vp.ValidFrom == null || vp.ValidFrom <= now) &&
                                            (vp.ValidTo == null || vp.ValidTo >= now) &&
                                            vp.DiscountPrice > 0)
                               .OrderByDescending(vp => vp.ValidFrom)
                               .Select(vp => vp.Price).FirstOrDefault(),
                    VariantCount = p.ProductVariants.Count(),
                    IsNew = true,
                    FreeShip = true
                })
                .ToListAsync();
            // 🧠 Lấy 8 biến thể bán chạy nhất
            var bestSellingVariants = await _context.OrderDetails
                .Where(od => od.ProductVariantID != 0)
                .GroupBy(od => od.ProductVariantID)
                .Select(g => new
                {
                    VariantID = g.Key,
                    TotalSold = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(x => x.TotalSold)
                .Take(8)
                .Join(
                    _context.productVariants
                        .Include(v => v.Product)
                        .Include(v => v.VariantPrices.Where(p => p.Status == PriceStatus.Active))
                        .Include(v => v.VariantImages.OrderBy(i => i.SortOrder)),
                    best => best.VariantID,
                    variant => variant.Id,
                    (best, variant) => new { variant, best.TotalSold }
                )
                .Where(x => x.variant.Status == VariantStatus.Active && !x.variant.Product.IsDeleted)
                .Select(x => new ProductCardVM
                {
                    ProductId = (int)x.variant.ProductId,
                    SKU = x.variant.SKU,
                    Name = x.variant.VariantName,
                    ShortDescription = x.variant.Product.ShortDescription,
                    Price = x.variant.VariantPrices.Select(p => p.DiscountPrice > 0 ? p.DiscountPrice : p.Price).FirstOrDefault(),
                    DiscountPrice = x.variant.VariantPrices.Select(p => p.DiscountPrice).FirstOrDefault(),
                    Images = x.variant.VariantImages.OrderBy(i => i.SortOrder).Select(i => i.Url).ToList()
                })
                .ToListAsync();


            var vm = new HomeIndexVM
            {
                Featured = featured,
                NewArrivals = newArrivals,
                BestSellingVariants = bestSellingVariants
            };

            return View(vm);
        }

        [HttpGet("/search")]
        public async Task<IActionResult> Searching(string? q, int page = 1, int pageSize = 24, CancellationToken ct = default)
        {
            q = (q ?? "").Trim();
            var terms = q.Length == 0 ? Array.Empty<string>() :
                        q.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var now = DateTime.UtcNow;

            // Base + Accessories (KHÔNG lọc theo giá)
            var baseQuery =
                from p in _context.Products.AsNoTracking()
                join a in _context.accessories.AsNoTracking()
                    on p.AccessoriesID equals a.AccessoriesID into gj
                from acc in gj.DefaultIfEmpty()
                select new { p, AccName = acc != null ? acc.Name : null };

            // Relevance: cộng điểm nếu trùng SKU / Slug / Accessories.Name
            var scored = baseQuery.Select(x => new
            {
                x.p.ProductID,
                x.p.SKU,
                x.p.Slug,
                x.p.ShortDescription,
                x.AccName,
                Score = 0
            });

            foreach (var t in terms)
            {
                var term = t;
                scored = scored.Select(s => new
                {
                    s.ProductID,
                    s.SKU,
                    s.Slug,
                    s.ShortDescription,
                    s.AccName,
                    Score = s.Score
                          + (EF.Functions.Like(s.SKU, $"%{term}%") ? 6 : 0)
                          + (EF.Functions.Like(s.Slug, $"%{term}%") ? 8 : 0)
                          + (EF.Functions.Like(s.AccName ?? "", $"%{term}%") ? 10 : 0)
                });
            }

            // Nếu không nhập gì: trả rỗng (hoặc bạn muốn trả tất cả thì bỏ where)
            if (terms.Length == 0)
                return View(new List<ProductCardVM>());

            var filtered = scored.Where(s => s.Score > 0);

            // Phân trang + sort theo relevance
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 12, 60);

            var pageQuery = filtered
                .OrderByDescending(s => s.Score)
                .ThenBy(s => s.Slug)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            // Map sang ProductCardVM (giá/ảnh/variants giống Index của bạn)
            var items = await
                (from s in pageQuery
                 join p in _context.Products.AsNoTracking() on s.ProductID equals p.ProductID
                 select new ProductCardVM
                 {
                     ProductId = (int)p.ProductID,
                     SKU = p.SKU,
                     Name = p.Slug,
                     ShortDescription = p.ShortDescription,

                     // tổng quan: lấy bản giá mới nhất (nullable)
            //         Price = p.ProductVariants
            //.OrderByDescending(vp => (DateTime?)vp.ValidFrom ?? DateTime.MinValue)
            //.Select(vp => (decimal?)vp.Price)
            //.FirstOrDefault() ?? 0m,
                     DiscountPrice = p.ProductVariants
                            .SelectMany(v => v.VariantPrices)
                            .OrderByDescending(vp => vp.ValidFrom ?? DateTime.MinValue)
                            .Select(vp => vp.DiscountPrice)
                            .FirstOrDefault(),

                     Images = p.ProductVariants
                            .SelectMany(v => v.VariantImages)
                            .OrderBy(i => i.SortOrder)
                            .Select(i => i.Url)
                            .ToList(),

                     Variants = p.ProductVariants
                            .Select(v => new VariantVM
                            {
                                VariantId = v.Id,
                                SKU = v.SKU,
                                Quantity = v.Quantity,
                                // VM là decimal (non-null) => ?? 0m
                                Price = v.VariantPrices
                                       .OrderByDescending(vp => vp.ValidFrom ?? DateTime.MinValue)
                                       .Select(vp => (decimal?)vp.Price)
                                       .FirstOrDefault() ?? 0m,

                                DiscountPrice = v.VariantPrices
                                        .OrderByDescending(vp => (DateTime?)vp.ValidFrom ?? DateTime.MinValue)
                                        .Select(vp => (decimal?)vp.DiscountPrice)  // ép về decimal?
                                        .FirstOrDefault() ?? 0m,
                                Images = v.VariantImages.Select(vi => vi.Url).ToList()
                            }).ToList()
                 }).ToListAsync(ct);

            return View(items); // View = List<ProductCardVM>
        }
        public async Task<IActionResult> NewArrivals(int take = 12, int newBadgeTopN = 100)
        {
            var now = DateTime.UtcNow;

            // 1) Tính ngưỡng ProductID cho badge "Mới"
            //    Ngưỡng = ProductID của sản phẩm hạng N khi sắp xếp theo ID giảm dần.
            //    Nếu tổng sản phẩm < N -> DefaultIfEmpty(long.MinValue) => mọi sản phẩm đều >= ngưỡng (đều "Mới").
            if (newBadgeTopN < 0) newBadgeTopN = 0; // an toàn
            long thresholdId = long.MaxValue; // nếu N = 0 thì không sản phẩm nào được coi là "Mới"
            if (newBadgeTopN > 0)
            {
                thresholdId = await _context.Products
                    .AsNoTracking()
                    .OrderByDescending(p => p.ProductID)
                    .Skip(newBadgeTopN - 1)
                    .Select(p => p.ProductID)
                    .DefaultIfEmpty(long.MinValue)
                    .FirstAsync();
            }

            // 2) Lấy danh sách "mới nhất" theo ProductID giảm dần
            var items = await _context.Products
                .AsNoTracking()
                .OrderByDescending(p => p.ProductID)   // coi ID lớn hơn là mới hơn
                .Take(take)
                .Select(p => new ProductListItemVM
                {
                    ProductId = (int)p.ProductID, // nếu trong domain là long, bạn có thể đổi VM sang long cho đồng bộ
                    SKU = p.SKU,
                    Title = string.IsNullOrEmpty(p.ShortDescription) ? p.Slug : p.ShortDescription,
                    Slug = p.Slug,

                    ImageUrl = p.ProductVariants
                        .SelectMany(v => v.VariantImages)
                        .OrderBy(i => i.SortOrder)
                        .Select(i => i.Url)
                        .FirstOrDefault() ?? "/images/placeholder.svg",

                    // Giá đang hiệu lực mới nhất
                    Currency = p.ProductVariants
                        .SelectMany(v => v.VariantPrices)
                        .Where(vp => (vp.ValidFrom == null || vp.ValidFrom <= now) &&
                                     (vp.ValidTo == null || vp.ValidTo >= now))
                        .OrderByDescending(vp => vp.ValidFrom)
                        .Select(vp => vp.Currency)
                        .FirstOrDefault() ?? "$",

                    // Giá cấp product: chọn dòng giá đang hiệu lực “mới nhất” trên mọi variant
                    Price = p.ProductVariants
                            .SelectMany(v => v.VariantPrices)
                            .Where(vp => (vp.ValidFrom == null || vp.ValidFrom <= now) &&
                                         (vp.ValidTo == null || vp.ValidTo >= now))
                            .OrderByDescending(vp => vp.ValidFrom)
                            .Select(vp => vp.DiscountPrice > 0 ? vp.DiscountPrice : vp.Price)
                            .FirstOrDefault(),

                    OldPrice = p.ProductVariants
                        .SelectMany(v => v.VariantPrices)
                        .Where(vp => (vp.ValidFrom == null || vp.ValidFrom <= now) &&
                                     (vp.ValidTo == null || vp.ValidTo >= now) &&
                                     vp.DiscountPrice > 0)
                        .OrderByDescending(vp => vp.ValidFrom)
                        .Select(vp => vp.Price)
                        .FirstOrDefault(),

                    VariantCount = p.ProductVariants.Count(),

                    // 3) Đánh dấu "Mới" nếu ProductID >= ngưỡng
                    IsNew = p.ProductID >= thresholdId,
                    FreeShip = true
                })
                .ToListAsync();

            return View("NewArrivals", items);
        }
    }
}
