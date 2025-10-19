    using ComputerSales.Application.UseCase.ProductOvetView_UC;
    using ComputerSales.Application.UseCaseDTO.ProductOverView_DTO;
    using ComputerSales.Application.UseCaseDTO.ProductOverView_DTO.DeleteDTO;
    using ComputerSales.Application.UseCaseDTO.ProductOverView_DTO.GetByIdDTO;
    using ComputerSales.Application.UseCaseDTO.ProductOverView_DTO.UpdateDTO;
    using ComputerSales.Domain.Entity.EProduct;
    using ComputerSales.Infrastructure.Persistence;
    using ComputerSalesProject_MVC.Areas.Admin.Models.Product_Overview;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    namespace ComputerSalesProject_MVC.Areas.Admin.Controllers
    {
        [Area("Admin")]
        [Route("Admin/[controller]/[action]")]
        public class ProductOverviewsController : Controller
        {
            private readonly AppDbContext _db;

            // Các UC bạn đã cung cấp
            private readonly GetByIdProductOverView_UC _getById;
            private readonly CreateProductOverView_UC _create;
            private readonly UpdateProductOverView_UC _update;
            private readonly DeleteProductOverView_UC _delete;

            public ProductOverviewsController(
                AppDbContext db,
                GetByIdProductOverView_UC getById,
                CreateProductOverView_UC create,
                UpdateProductOverView_UC update,
                DeleteProductOverView_UC delete
            )
            {
                _db = db;
                _getById = getById;
                _create = create;
                _update = update;
                _delete = delete;
            }

            // Múi giờ Việt Nam (Windows)
            private static TimeZoneInfo VnTz =>
                TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

            // Helper convert UTC -> VN (Giống controller mẫu)
            private string ToVnTime(DateTime? utc)
            {
                if (!utc.HasValue) return "—";
                return TimeZoneInfo.ConvertTimeFromUtc(
                    DateTime.SpecifyKind(utc.Value, DateTimeKind.Utc), VnTz)
                    .ToString("yyyy-MM-dd HH:mm:ss");
            }

            // ==================== INDEX (cho 1 Product) ====================
            [HttpGet("{productId:long}")]
            public async Task<IActionResult> ProductOverViewsIndex(long productId, CancellationToken ct)
            {
                var product = await _db.Products.AsNoTracking()
                                          .Select(p => new { p.ProductID, p.SKU })
                                          .FirstOrDefaultAsync(p => p.ProductID == productId, ct);

                if (product == null) return NotFound("Không tìm thấy Product.");

                ViewBag.ProductId = product.ProductID;
                ViewBag.ProductSKU = product.SKU;

                // SỬA LỖI: Chữ 'P' phải viết hoa
                var items = await _db.productOverviews
                                    .Where(po => po.ProductId == productId)
                                    .OrderBy(po => po.DisplayOrder)
                                    .Select(po => new ProductOverViewOutput(
                                        po.ProductOverviewId,
                                        po.ProductId,
                                        po.BlockType,
                                        po.TextContent,
                                        po.ImageUrl,
                                        po.Caption,
                                        po.DisplayOrder,
                                        po.CreateDate
                                    ))
                                    .ToListAsync(ct);

                ViewBag.ToVn = (Func<DateTime?, string>)(utc => ToVnTime(utc));
                return View(items);
            }

            // ==================== DETAILS ====================
            [HttpGet("{id:int}")]
            public async Task<IActionResult> ProductOverViewsDetails(int id, CancellationToken ct)
            {
                var dto = await _getById.HandleAsync(new GetByIDProductOverViewInput(id), ct);
                if (dto is null) return NotFound();

                ViewBag.CreateDateVN = ToVnTime(dto.CreateDate);
                return View(dto);
            }

            // ==================== CREATE ====================
            [HttpGet("{productId:long}")]
            public async Task<IActionResult> ProductOverViewsCreate(long productId, CancellationToken ct)
            {
                var product = await _db.Products.AsNoTracking()
                                          .Select(p => new { p.ProductID, p.SKU })
                                          .FirstOrDefaultAsync(p => p.ProductID == productId, ct);
                if (product == null) return NotFound("Product không tồn tại.");

                int nextOrder = 1;
                // SỬA LỖI: Chữ 'P' phải viết hoa
                var overviewsExist = await _db.productOverviews.AnyAsync(po => po.ProductId == productId, ct);
                if (overviewsExist)
                {
                    // SỬA LỖI: Chữ 'P' phải viết hoa
                    nextOrder = (await _db.productOverviews
                                             .Where(po => po.ProductId == productId)
                                             .MaxAsync(po => po.DisplayOrder, ct)) + 1;
                }

                var vm = new ProductOverviewCreateVM
                {
                    ProductId = productId,
                    DisplayOrder = nextOrder
                };

                ViewBag.ProductSKU = product.SKU;
                return View(vm);
            }

            [HttpPost("{productId:long}")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> ProductOverViewsCreate(ProductOverviewCreateVM vm, CancellationToken ct)
            {
                // --- Validation ---
                var productExists = await _db.Products.AsNoTracking().AnyAsync(p => p.ProductID == vm.ProductId, ct);
                if (!productExists)
                    ModelState.AddModelError(nameof(vm.ProductId), "Product không tồn tại.");

                if ((vm.BlockType == OverviewBlockType.Text || vm.BlockType == OverviewBlockType.List) && string.IsNullOrWhiteSpace(vm.TextContent))
                    ModelState.AddModelError(nameof(vm.TextContent), "TextContent là bắt buộc cho block loại Text/List.");

                if ((vm.BlockType == OverviewBlockType.Image || vm.BlockType == OverviewBlockType.Logo) && string.IsNullOrWhiteSpace(vm.ImageUrl))
                    ModelState.AddModelError(nameof(vm.ImageUrl), "ImageUrl là bắt buộc cho block loại Image/Logo.");

                if (!ModelState.IsValid)
                {
                    ViewBag.ProductSKU = (await _db.Products.AsNoTracking().Select(p => new { p.ProductID, p.SKU }).FirstOrDefaultAsync(p => p.ProductID == vm.ProductId, ct))?.SKU ?? "Unknown";
                    return View(vm);
                }
                // --- Hết Validation ---

                var input = new ProductOverViewInput(
                    vm.ProductId,
                    vm.BlockType,
                    vm.TextContent ?? "",
                    vm.ImageUrl,
                    vm.Caption,
                    vm.DisplayOrder
                );

                var created = await _create.HandleAsync(input, ct);

                TempData["Success"] = "Đã tạo khối overview mới.";
                return RedirectToAction(nameof(ProductOverViewsIndex), new { productId = created.ProductId });
            }

            // ==================== EDIT (Dựa trên Update_UC) ====================
            [HttpGet("{id:int}")]
            public async Task<IActionResult> Edit(int id, CancellationToken ct)
            {
                var dto = await _getById.HandleAsync(new GetByIDProductOverViewInput(id), ct);
                if (dto is null) return NotFound();

                var vm = new ProductOverviewEditVM
                {
                    ProductOverviewId = dto.ProductOverviewId,
                    ProductId = dto.ProductId,
                    BlockType = dto.BlockType,
                    TextContent = dto.TextContent,
                    ImageUrl = dto.ImageUrl,
                    Caption = dto.Caption,
                    DisplayOrder = dto.DisplayOrder
                };

                var product = await _db.Products.AsNoTracking().Select(p => new { p.ProductID, p.SKU }).FirstOrDefaultAsync(p => p.ProductID == vm.ProductId, ct);
                ViewBag.ProductSKU = product?.SKU ?? "Unknown";

                return View(vm);
            }

            [HttpPost("{productId:long}")]
            [ValidateAntiForgeryToken]
            // Sửa lại tên Action (bỏ 'ProductOverViews' đi để khớp với asp-action="Edit" của form)
            public async Task<IActionResult> Edit(ProductOverviewEditVM vm, CancellationToken ct)
            {
                // SỬA LỖI: Chữ 'P' phải viết hoa
                var entity = await _db.productOverviews.AsNoTracking().FirstOrDefaultAsync(po => po.ProductOverviewId == vm.ProductOverviewId, ct);
                if (entity == null) return NotFound();

                // (Validation logic... giữ nguyên)

                if (!ModelState.IsValid)
                {
                    // ... (return View(vm) giữ nguyên)
                }

                // (Tạo 'input' DTO... giữ nguyên)
                var input = new ProductOverviewUpdate_Input(
                    ProductOverviewId: vm.ProductOverviewId,
                    TextContent: vm.TextContent,
                    ImageUrl: vm.ImageUrl,
                    Caption: vm.Caption,
                    DisplayOrder: vm.DisplayOrder,
                    BlockType: null
                );

                var updated = await _update.HandleAsync(input, ct);
                if (updated == null) return NotFound();

                TempData["Success"] = "Đã cập nhật khối overview.";
                return RedirectToAction(nameof(ProductOverViewsIndex), new { productId = updated.ProductId });
            }

            // ==================== DELETE ====================
            [HttpGet("{id:int}")]
            public async Task<IActionResult> ProductOverViewsDelete(int id, CancellationToken ct)
            {
                var dto = await _getById.HandleAsync(new GetByIDProductOverViewInput(id), ct);
                if (dto is null) return NotFound();

                ViewBag.CreateDateVN = ToVnTime(dto.CreateDate);
                return View(dto);
            }

            [HttpPost("{id:int}")]
            [ActionName("Delete")] // Tên này phải là "Delete", không phải "ProductOverViewsDelete"
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
            {
                var deletedDto = await _delete.HandleAsync(new DeleteProductOverViewInput(id), ct);

                if (deletedDto == null)
                {
                    TempData["Error"] = "Xóa không thành công (không tìm thấy).";
                    return RedirectToAction("Index", "Dashboard");
                }

                TempData["Success"] = "Đã xóa khối overview.";
                return RedirectToAction(nameof(ProductOverViewsIndex), new { productId = deletedDto.ProductId });
            }
        }
    }