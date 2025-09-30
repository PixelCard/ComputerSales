using ComputerSales.Application.UseCase.Product_UC;
using ComputerSales.Application.UseCase.ProductVariant_UC;
using ComputerSales.Application.UseCaseDTO.Product_DTO;
using ComputerSales.Application.UseCaseDTO.ProductVariant_DTO;
using ComputerSales.Domain.Entity.EProduct; // ProductStatus
using ComputerSales.Infrastructure.Persistence; // AppDbContext
using ComputerSalesProject_MVC.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ComputerSalesProject_MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _db;
        private readonly CreateProduct_UC _createUC;
        private readonly CreateProductVariant_UC _createVariantUC;

        public ProductController(AppDbContext db, CreateProduct_UC createUC, CreateProductVariant_UC createVariantUC)
        {
            _db = db;
            _createUC = createUC;
            _createVariantUC = createVariantUC;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string? q, string? status, int page = 1, int pageSize = 20, CancellationToken ct = default)
        {
            if (page < 1) page = 1;
            if (pageSize <= 0 || pageSize > 200) pageSize = 20;

            var query = _db.Set<Product>().AsNoTracking().Where(p => !p.IsDeleted);

            // lọc theo status nếu có
            if (!string.IsNullOrWhiteSpace(status) &&
                Enum.TryParse<ProductStatus>(status, true, out var st))
            {
                query = query.Where(p => p.Status == st);
            }

            var total = await query.CountAsync(ct);
            var items = await query
                .OrderByDescending(p => p.ProductID)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductRowVM
                {
                    ProductID = p.ProductID,
                    SKU = p.SKU,
                    Slug = p.Slug,
                    ShortDescription = p.ShortDescription,
                    Status = p.Status,
                    VariantsCount = p.ProductVariants.Count,
                    ProviderName = p.Provider.ProviderName,
                    AccessoriesName = p.Accessories.Name
                })
                .ToListAsync(ct);

            var vm = new ProductIndexVM
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalItems = total,
                Query = q,
                Status = status // <- giữ lại để View hiển thị đúng selected
            };

            return View(vm);
        }

        // Nạp dropdown cho View (Providers, Accessories, Status=int)
        private async Task LoadLookupsAsync(CancellationToken ct = default)
        {
            var providers = await _db.Providers
                .AsNoTracking()
                .Select(x => new { x.ProviderID, x.ProviderName })
                .ToListAsync(ct);

            var accessories = await _db.accessories
                .AsNoTracking()
                .Select(x => new { x.AccessoriesID, x.Name })
                .ToListAsync(ct);

            ViewBag.ProviderList = new SelectList(providers, "ProviderID", "ProviderName");
            ViewBag.AccessoriesList = new SelectList(accessories, "AccessoriesID", "Name");
            ViewData["DbgCounts"] = $"Providers={providers.Count}, Accessories={accessories.Count}";

            // Status: enum -> int cho View (DTO nhận int)
            ViewBag.StatusList = new SelectList(new[]
            {
                new { Value = (int)ProductStatus.Inactive, Text = nameof(ProductStatus.Inactive) },
                new { Value = (int)ProductStatus.Active,   Text = nameof(ProductStatus.Active) }
            }, "Value", "Text");
        }

        // GET /Admin/Product/Create
        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            await LoadLookupsAsync(ct);
            return View(); // @model ProductDTOInput
        }

        // POST /Admin/Product/Create (tạo qua form)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductDTOInput input, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await LoadLookupsAsync(ct);
                return View(input);
            }

            try
            {
                // Gọi thẳng UseCase, không chỉnh DTO/UC
                ProductOutputDTOcs output = await _createUC.HandleAsync(input, ct);

                TempData["Success"] = "Tạo sản phẩm thành công.";
                // sau khi _createUC.HandleAsync(...) trả về output.ProductID
                return RedirectToAction(nameof(CreateVariant), new { productId = output.ProductID });

                return View(output);
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Lỗi: {ex.Message}");
            }

            await LoadLookupsAsync(ct);
            return View(input);
        }
        [HttpGet]
        public async Task<IActionResult> CreateVariant(long productId, CancellationToken ct)
        {
            // Lấy Product để hiển thị thông tin ở View (tên SP...)
            var product = await _db.Set<Product>().FindAsync(new object?[] { productId }, ct);
            if (product is null) return NotFound();

            // [THÊM] Lấy danh sách biến thể để render dưới form
            var variants = await _db.Set<ProductVariant>()
                .AsNoTracking()
                .Where(v => v.ProductId == productId)
                .OrderByDescending(v => v.Id)
                .Select(v => new
                {
                    v.Id,
                    v.SKU,
                    v.VariantName,
                    v.Status,
                    v.Quantity
                })
                .ToListAsync(ct);
            ViewBag.Variants = variants;                 // [THÊM]
            ViewBag.VariantsCount = variants.Count;      // [THÊM]

            // Truyền product sang View (nếu cần show)
            ViewBag.Product = product;
            LoadVariantStatusLookups();

            // Khởi tạo model mặc định cho form tạo variant
            var vm = new ProductVariantInput(
                ProductId: productId,
                SKU: string.Empty,
                Status: VariantStatus.Active,
                Quantity: 0,
                VariantName: string.Empty
            );

            return View(vm); // @model ProductVariantInput
        }

        private void LoadVariantStatusLookups()
        {
            ViewBag.VariantStatusList = new SelectList(new[]
            {
                new { Value = (int)VariantStatus.Inactive, Text = nameof(VariantStatus.Inactive) },
                new { Value = (int)VariantStatus.Active,   Text = nameof(VariantStatus.Active) }
            }, "Value", "Text");
        }

        // POST /Admin/Product/CreateVariant
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVariant(ProductVariantInput input, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                // Nạp lại dữ liệu phụ trợ
                var product = await _db.Set<Product>().FindAsync(new object?[] { input.ProductId }, ct);
                ViewBag.Product = product;
                LoadVariantStatusLookups();
                return View(input);
            }

            try
            {
                ProductVariantOutput output = await _createVariantUC.HandleAsync(input, ct);
                TempData["Success"] = "Tạo biến thể sản phẩm thành công.";

                // Bạn có thể:
                // 1) Ở lại trang để nhập tiếp nhiều biến thể:
                //    return RedirectToAction(nameof(CreateVariant), new { productId = input.ProductId });
                // 2) Quay về trang chi tiết sản phẩm:
                // Sau khi tạo thành công
                TempData["Success"] = "Tạo biến thể sản phẩm thành công.";
                return RedirectToAction(nameof(CreateVariant), new { productId = input.ProductId });

            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Lỗi: {ex.Message}");
            }

            var p = await _db.Set<Product>().FindAsync(new object?[] { input.ProductId }, ct);
            ViewBag.Product = p;
            LoadVariantStatusLookups();
            return View(input);
        }


    }
}
