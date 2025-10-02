using ComputerSales.Application.UseCase.Product_UC;
using ComputerSales.Application.UseCase.ProductVariant_UC;
using ComputerSales.Application.UseCaseDTO.Product_DTO;
using ComputerSales.Application.UseCaseDTO.ProductVariant_DTO;
using ComputerSales.Domain.Entity.EProduct; // ProductStatus
using ComputerSales.Infrastructure.Persistence; // AppDbContext
using ComputerSalesProject_MVC.Areas.Admin.Models;
using ComputerSalesProject_MVC.Areas.Admin.Models.NewFolder;
using ComputerSalesProject_MVC.Areas.Admin.Models.ProductVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
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


        // Index hiển thị danh sách sản phẩm 
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

        //============================= Product Details =============================//
        [HttpGet]
        public async Task<IActionResult> ProductDetails(long id, CancellationToken ct)
        {
            var product = await _db.Set<Product>()
                .AsNoTracking()
                .Include(p => p.Provider)
                .Include(p => p.Accessories)
                .Include(p => p.ProductVariants)
                .FirstOrDefaultAsync(p => p.ProductID == id && !p.IsDeleted, ct);

            if (product is null)
                return NotFound();

            // Chuẩn hóa sang ViewModel để hiển thị
            var vm = new ProductDetailsVM
            {
                ProductID = product.ProductID,
                ShortDescription = product.ShortDescription,
                SKU = product.SKU,
                Slug = product.Slug,
                Status = product.Status,
                ProviderName = product.Provider?.ProviderName ?? "(N/A)",
                AccessoriesName = product.Accessories?.Name ?? "(N/A)",
                Variants = product.ProductVariants
                    .OrderByDescending(v => v.Id)
                    .Select(v => new ProductVariantDetailVM
                    {
                        Id = v.Id,
                        SKU = v.SKU,
                        ProductId = v.ProductId,
                        VariantName = v.VariantName,
                        Quantity = v.Quantity,
                        Status = v.Status
                    })
                    .ToList()
            };

            return View(vm);
        }
        //=================== Edit Product  ========================//
        // GET: /Admin/Product/Edit/5
        [HttpGet]
        public async Task<IActionResult> UpdateProduct(long id, CancellationToken ct)
        {
            var product = await _db.Set<Product>()
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProductID == id && !p.IsDeleted, ct);

            if (product == null) return NotFound();

            // map sang DTO input để hiển thị trong form
            var input = new ProductDTOInput(
                ShortDescription: product.ShortDescription,
                Status: (int)product.Status,
                AccessoriesID: product.AccessoriesID,
                ProviderID: product.ProviderID,
                Slug: product.Slug,
                SKU: product.SKU
            );

            ViewBag.ProductId = product.ProductID;
            await LoadLookupsAsync(ct);
            return View(input);
        }

        // POST: /Admin/Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProduct(long id, ProductDTOInput input, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ProductId = id;
                await LoadLookupsAsync(ct);
                return View(input);
            }

            var product = await _db.Set<Product>().FirstOrDefaultAsync(p => p.ProductID == id && !p.IsDeleted, ct);
            if (product == null) return NotFound();

            try
            {
                product.ShortDescription = input.ShortDescription;
                product.Status = (ProductStatus)input.Status;
                product.AccessoriesID = input.AccessoriesID;
                product.ProviderID = input.ProviderID;
                product.Slug = input.Slug;
                product.SKU = input.SKU;

                _db.Update(product);
                await _db.SaveChangesAsync(ct);

                TempData["Success"] = "Cập nhật sản phẩm thành công.";
                return RedirectToAction(nameof(ProductDetails), new { id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Lỗi: {ex.Message}");
                await LoadLookupsAsync(ct);
                return View(input);
            }
        }
        //=========================================================================================//
        // GET: /Admin/Product/Delete/5
        [HttpGet]
        public async Task<IActionResult> DeleteProduct(long id, CancellationToken ct)
        {
            var product = await _db.Set<Product>()
                .AsNoTracking()
                .Include(p => p.Provider)
                .Include(p => p.Accessories)
                .FirstOrDefaultAsync(p => p.ProductID == id && !p.IsDeleted, ct);

            if (product == null) return NotFound();

            var vm = new ProductDetailsVM
            {
                ProductID = product.ProductID,
                ShortDescription = product.ShortDescription,
                SKU = product.SKU,
                Slug = product.Slug,
                Status = product.Status,
                ProviderName = product.Provider?.ProviderName ?? "(N/A)",
                AccessoriesName = product.Accessories?.Name ?? "(N/A)"
            };

            return View(vm); // hiển thị confirm "Bạn có chắc muốn xóa không?"
        }

        // POST: /Admin/Product/DeleteConfirmed/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id, CancellationToken ct)
        {
            var product = await _db.Set<Product>().FirstOrDefaultAsync(p => p.ProductID == id && !p.IsDeleted, ct);
            if (product == null) return NotFound();

            try
            {
                // soft delete
                product.IsDeleted = true;
                _db.Update(product);
                await _db.SaveChangesAsync(ct);

                TempData["Success"] = "Sản phẩm đã được xóa (soft delete).";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi xóa: {ex.Message}";
                return RedirectToAction(nameof(Delete), new { id });
            }
        }




    }
}
