using ComputerSales.Application.UseCase.Product_UC;
using ComputerSales.Application.UseCase.ProductVariant_UC;
using ComputerSales.Application.UseCaseDTO.Product_DTO;
using ComputerSales.Application.UseCaseDTO.ProductOverView_DTO;
using ComputerSales.Application.UseCaseDTO.ProductVariant_DTO;
using ComputerSales.Domain.Entity.E_Order;
using ComputerSales.Domain.Entity.EProduct; // ProductStatus
using ComputerSales.Domain.Entity.EVariant;
using ComputerSales.Infrastructure.Persistence; // AppDbContext
using ComputerSalesProject_MVC.Areas.Admin.Models;
using ComputerSalesProject_MVC.Areas.Admin.Models.NewFolder;
using ComputerSalesProject_MVC.Areas.Admin.Models.ProductVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ComputerSalesProject_MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
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
        private async Task LoadLookupsAsync(
             CancellationToken ct = default,
             long? selectedProviderId = null,
             long? selectedAccessoriesId = null,
             int? selectedStatus = null)
                {
                    var providers = await _db.Providers.AsNoTracking()
                        .Select(x => new { x.ProviderID, x.ProviderName }).ToListAsync(ct);
                    var accessories = await _db.accessories.AsNoTracking()
                        .Select(x => new { x.AccessoriesID, x.Name }).ToListAsync(ct);

                    ViewBag.ProviderList = new SelectList(providers, "ProviderID", "ProviderName", selectedProviderId);
                    ViewBag.AccessoriesList = new SelectList(accessories, "AccessoriesID", "Name", selectedAccessoriesId);
                    ViewBag.StatusList = new SelectList(new[]
                    {
                new { Value = (int)ProductStatus.Inactive, Text = nameof(ProductStatus.Inactive) },
                new { Value = (int)ProductStatus.Active,   Text = nameof(ProductStatus.Active) }
             }, "Value", "Text", selectedStatus);
        }


        // GET /Admin/Product/Create
        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            await LoadLookupsAsync(ct);
            return View(); // @model ProductDTOInput
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductDTOInput input, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await LoadLookupsAsync(ct, input.ProviderID, input.AccessoriesID, input.Status);
                return View(input);
            }

            //Fix reload lại trang mất hết dropdown
            if (!ModelState.IsValid)
            {
                await LoadLookupsAsync(ct, input.ProviderID, input.AccessoriesID, input.Status);
                return View(input);
            }

            var finalSku = await EnsureFinalSkuAsync(input.SKU, ct);

            if (finalSku == null)
            {
                ModelState.AddModelError("SKU", "Mã SKU này đã tồn tại, vui lòng nhập mã khác.");
                await LoadLookupsAsync(ct, input.ProviderID, input.AccessoriesID, input.Status);
                return View(input);
            }

            var product = Product.Create(
                accessoriesId: input.AccessoriesID,
                providerId: input.ProviderID,
                shortDescription: input.ShortDescription,
                sku: finalSku,
                slug: input.Slug
            );

            _db.Products.Add(product);
            try
            {
                await _db.SaveChangesAsync(ct);
            }
            catch
            {
                // nếu lỗi lưu, cũng cần nạp lại dropdown trước khi trả về view
                await LoadLookupsAsync(ct, input.ProviderID, input.AccessoriesID, input.Status);
                ModelState.AddModelError(string.Empty, "Có lỗi khi lưu dữ liệu.");
                return View(input);
            }

            TempData["Success"] = $"✅ Đã tạo sản phẩm mới ({finalSku}) thành công!";
            return RedirectToAction("Index", "Product", new { area = "Admin" });
        }


        // Trả về SKU cuối cùng.
        // - Nếu người dùng để trống -> tự sinh.
        // - Nếu người dùng nhập mà bị trùng -> trả về null.
        private async Task<string?> EnsureFinalSkuAsync(string? inputSku, CancellationToken ct)
        {
            string finalSku;

            // 1) Nếu SKU trống -> tự sinh
            if (string.IsNullOrWhiteSpace(inputSku))
            {
                var lastId = await _db.Products
                                      .AsNoTracking()
                                      .MaxAsync(p => (int?)p.ProductID, ct) ?? 0;
                finalSku = $"SKU_{10000 + lastId + 1}";
            }
            else
            {
                // 2) Có nhập -> chuẩn hóa
                finalSku = inputSku.Trim();

                // 3) Kiểm tra trùng
                var exists = await _db.Products
                                      .AsNoTracking()
                                      .AnyAsync(p => p.SKU == finalSku, ct);
                if (exists)
                    return null; // báo trùng
            }

            return finalSku; // hợp lệ
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
            ViewBag.Variants = variants;                 
            ViewBag.VariantsCount = variants.Count;

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
                .Include(p => p.ProductOverviews)               
                .FirstOrDefaultAsync(p => p.ProductID == id && !p.IsDeleted, ct);

            if (product is null)
                return NotFound();

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
                    .ToList(),

                // <<-- THÊM MAP OVERVIEW VÀO VM (1-1)
                ProductOverviews = product.ProductOverviews != null 
                    ? new List<ProductOverViewOutput> 
                    { 
                        new ProductOverViewOutput(
                            product.ProductOverviews.ProductOverviewId,
                            product.ProductOverviews.ProductId,
                            product.ProductOverviews.TextContent,
                            product.ProductOverviews.CreateDate
                        )
                    }
                    : new List<ProductOverViewOutput>()
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


    private static bool IsExpired(VariantPrice p, DateTime nowUtc)
    {
        if (p == null) return true;

        // Hết hạn khi: chưa đến ValidFrom, hoặc đã qua ValidTo, hoặc Inactive
        if (p.Status != PriceStatus.Active) return true;
        if (p.ValidFrom.HasValue && p.ValidFrom.Value > nowUtc) return true;
        if (p.ValidTo.HasValue && p.ValidTo.Value < nowUtc) return true;

        return false;
    }

    private static decimal EffectiveDiscount(VariantPrice p, DateTime nowUtc)
    {
        // Nếu hết hạn → discount = 0
        return IsExpired(p, nowUtc) ? 0m : (p.DiscountPrice <= 0m ? 0m : p.DiscountPrice);
    }

    /// <summary>
    /// Tính giá hiển thị theo quy tắc:
    /// - Discount 0..100 => % khuyến mãi (price - price * %/100)
    /// - Discount > 100  => là giá đã giảm (final price)
    /// - Hết hạn / Inactive => discount = 0 (dùng giá gốc)
    /// </summary>
    private static (decimal price, decimal? old, string currency) ResolveDisplayPrice(VariantPrice? row, DateTime? nowUtc = null)
    {
        if (row == null) return (0m, null, "VND");

        var now = nowUtc ?? DateTime.UtcNow;
        var discount = EffectiveDiscount(row, now);  // <-- dùng hàm trên
        var currency = string.IsNullOrWhiteSpace(row.Currency) ? "VND" : row.Currency;

        if (discount <= 0m)
            return (row.Price, null, currency);

        if (discount <= 100m)
        {
            var final = Math.Round(row.Price * (1m - (discount / 100m)), 2, MidpointRounding.AwayFromZero);
            return (final, row.Price, currency);
        }

        // discount > 100 => discount là "giá sau giảm"
        var price = discount;
        var old = row.Price > price ? row.Price : (decimal?)null;
        return (price, old, currency);
    }
        // GET: /Admin/Product/Deleted
        [HttpGet]
        public async Task<IActionResult> DanhSachSanPhamXoa(int page = 1, int pageSize = 20, CancellationToken ct = default)
        {
            if (page < 1) page = 1;
            if (pageSize <= 0 || pageSize > 200) pageSize = 20;

            var query = _db.Set<Product>()
                           .AsNoTracking()
                           .Where(p => p.IsDeleted); // 🔥 lấy sản phẩm đã xóa mềm

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
                    ProviderName = p.Provider.ProviderName,
                    AccessoriesName = p.Accessories.Name,
                    VariantsCount = p.ProductVariants.Count
                })
                .ToListAsync(ct);

            var vm = new ProductIndexVM
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalItems = total,
                Query = null,
                Status = "Deleted"
            };

            ViewData["Title"] = "Danh sách sản phẩm đã xóa";
            return View("DanhSachSanPhamXoa", vm);
        }

        //hàm khôi phục sản phẩm đã xóa đổi IsDelete -> false trong layout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(long id, CancellationToken ct)
        {
            var product = await _db.Set<Product>().FirstOrDefaultAsync(p => p.ProductID == id && p.IsDeleted, ct);
            if (product == null)
            {
                TempData["Error"] = "Không tìm thấy sản phẩm để khôi phục.";
                return RedirectToAction(nameof(DanhSachSanPhamXoa));
            }

            product.IsDeleted = false;
            _db.Update(product);
            await _db.SaveChangesAsync(ct);

            TempData["Success"] = "Khôi phục sản phẩm thành công.";
            return RedirectToAction(nameof(DanhSachSanPhamXoa));
        }

        // POST: không xóa sản phẩm , chỉ cập nhật trạng IsDelete ẩn đi sau đó mới xác nhận xóa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id, CancellationToken ct)
        {
            var product = await _db.Set<Product>()
                .FirstOrDefaultAsync(p => p.ProductID == id && !p.IsDeleted, ct);

            if (product == null)
            {
                TempData["Error"] = "Không tìm thấy sản phẩm cần xóa.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                // ✅ Đánh dấu IsDeleted = true
                product.IsDeleted = true;
                _db.Update(product);
                await _db.SaveChangesAsync(ct);

                TempData["Success"] = "✅ Đã xóa sản phẩm thành công (soft delete).";

                // ✅ Quay về danh sách
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"❌ Lỗi khi xóa sản phẩm: {ex.Message}";
                return RedirectToAction(nameof(DeleteProduct), new { id });
            }
        }


        //hàm xác nhận xóa sản phẩm cho view DanhSachSanPhamXoa -> Xác nhận xóa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePermanently(long id, CancellationToken ct)
        {
            var product = await _db.Set<Product>()
                .Include(p => p.ProductVariants)
                .FirstOrDefaultAsync(p => p.ProductID == id && p.IsDeleted, ct);

            if (product == null)
            {
                TempData["Error"] = "❌ Không thể xóa: Sản phẩm không tồn tại hoặc chưa bị xóa mềm.";
                return RedirectToAction(nameof(DanhSachSanPhamXoa));
            }

            try
            {
                var variantIds = product.ProductVariants.Select(v => v.Id).ToList();

                bool hasOrder = await _db.Set<OrderDetail>()
                    .AnyAsync(od => od.ProductID == id || variantIds.Contains(od.ProductVariantID), ct);

                if (hasOrder)
                {
                    TempData["Error"] = "⚠️ Không thể xóa vĩnh viễn vì sản phẩm này còn đơn hàng.";
                    return RedirectToAction(nameof(DanhSachSanPhamXoa));
                }

                _db.Remove(product);
                await _db.SaveChangesAsync(ct);

                TempData["Success"] = "🗑️ Đã xóa vĩnh viễn sản phẩm khỏi hệ thống.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"❌ Lỗi khi xóa sản phẩm: {ex.Message}";
            }

            return RedirectToAction(nameof(DanhSachSanPhamXoa));
        }


    }
}
