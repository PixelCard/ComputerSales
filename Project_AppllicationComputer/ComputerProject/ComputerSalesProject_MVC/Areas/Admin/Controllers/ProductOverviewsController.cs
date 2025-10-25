using ComputerSales.Application.UseCase.ProductOvetView_UC;
using ComputerSales.Application.UseCaseDTO.ProductOverView_DTO;
using ComputerSales.Application.UseCaseDTO.ProductOverView_DTO.DeleteDTO;
using ComputerSales.Application.UseCaseDTO.ProductOverView_DTO.GetByIdDTO;
using ComputerSales.Application.UseCaseDTO.ProductOverView_DTO.UpdateDTO;
using ComputerSales.Infrastructure.Persistence;
using ComputerSalesProject_MVC.Areas.Admin.Models.Product_Overview;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComputerSalesProject_MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class ProductOverviewsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _configuration;

        // Các UC bạn đã cung cấp
        private readonly GetByIdProductOverView_UC _getById;
        private readonly CreateProductOverView_UC _create;
        private readonly UpdateProductOverView_UC _update;
        private readonly DeleteProductOverView_UC _delete;

        public ProductOverviewsController(
            AppDbContext db,
            IConfiguration configuration,
            GetByIdProductOverView_UC getById,
            CreateProductOverView_UC create,
            UpdateProductOverView_UC update,
            DeleteProductOverView_UC delete
        )
        {
            _db = db;
            _configuration = configuration;
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

        [HttpGet]
        public IActionResult ProductOverViewsCreate(long productId)
        {
            ViewBag.TinyMCEApiKey = _configuration["TinyMCE:ApiKey"];
            return View(new ProductOverviewCreateVM { ProductId = productId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductOverViewsCreate(ProductOverviewCreateVM vm,CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(vm);

            if(string.IsNullOrEmpty(vm.TextContent))
            {
                ModelState.AddModelError(string.Empty, "TextContent is required.");
                return View(vm);
            }

            try
            {
                await _create.HandleAsync(new ProductOverViewInput(vm.ProductId, vm.TextContent), ct);
                TempData["Success"] = "Tạo Product Overview thành công!";
                return RedirectToAction("ProductDetails", "Product", new { id = vm.ProductId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Lỗi: {ex.Message}");
                return View(vm);
            }
        }

        // GET: ProductOverViewsIndex
        [HttpGet]
        public async Task<IActionResult> ProductOverViewsIndex(long productId, CancellationToken ct)
        {
            var product = await _db.Products
                .AsNoTracking()
                .Include(p => p.ProductOverviews)
                .FirstOrDefaultAsync(p => p.ProductID == productId, ct);

            if (product == null)
            {
                TempData["Error"] = "Không tìm thấy sản phẩm.";
                return RedirectToAction("Index", "Product");
            }

            ViewBag.ProductId = productId;
            ViewBag.ProductName = product.ShortDescription;

            var overviews = product.ProductOverviews != null
                    ? new List<ProductOverViewOutput>
                    {
                        new ProductOverViewOutput(
                            product.ProductOverviews.ProductOverviewId,
                            product.ProductOverviews.ProductId,
                            product.ProductOverviews.TextContent,
                            product.ProductOverviews.CreateDate
                        )
                    }
                    : new List<ProductOverViewOutput>();

            return View(overviews);
        }

        // GET: ProductOverViewsDetails
        [HttpGet]
        public async Task<IActionResult> ProductOverViewsDetails(int id, CancellationToken ct)
        {
            try
            {
                var result = await _getById.HandleAsync(new GetByIDProductOverViewInput(id), ct);
                
                if (result == null)
                {
                    TempData["Error"] = "Không tìm thấy Product Overview.";
                    return RedirectToAction("Index", "Product");
                }

                return View(result);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi: {ex.Message}";
                return RedirectToAction("Index", "Product");
            }
        }

        // GET: Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            try
            {
                var result = await _getById.HandleAsync(new GetByIDProductOverViewInput(id), ct);
                
                if (result == null)
                {
                    TempData["Error"] = "Không tìm thấy Product Overview.";
                    return RedirectToAction("Index", "Product");
                }

                ViewBag.TinyMCEApiKey = _configuration["TinyMCE:ApiKey"];

                var vm = new ProductOverviewEditVM
                {
                    ProductOverviewId = result.ProductOverviewId,
                    ProductId = result.ProductId,
                    TextContent = result.TextContent
                };

                return View(vm);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi: {ex.Message}";
                return RedirectToAction("Index", "Product");
            }
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductOverviewEditVM vm, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(vm);

            if (string.IsNullOrEmpty(vm.TextContent))
            {
                ModelState.AddModelError(string.Empty, "TextContent is required.");
                return View(vm);
            }

            try
            {
                await _update.HandleAsync(new ProductOverviewUpdate_Input(vm.ProductOverviewId, vm.TextContent), ct);
                TempData["Success"] = "Cập nhật Product Overview thành công!";
                return RedirectToAction("ProductDetails", "Product", new { id = vm.ProductId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Lỗi: {ex.Message}");
                return View(vm);
            }
        }

        // GET: ProductOverViewsDelete
        [HttpGet]
        public async Task<IActionResult> ProductOverViewsDelete(int id, CancellationToken ct)
        {
            try
            {
                var result = await _getById.HandleAsync(new GetByIDProductOverViewInput(id), ct);
                
                if (result == null)
                {
                    TempData["Error"] = "Không tìm thấy Product Overview.";
                    return RedirectToAction("Index", "Product");
                }

                return View(result);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi: {ex.Message}";
                return RedirectToAction("Index", "Product");
            }
        }

        // POST: ProductOverViewsDelete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductOverViewsDeleteConfirmed(int id, CancellationToken ct)
        {
            try
            {
                // Lấy ProductId trước khi xóa để redirect
                var overview = await _db.productOverviews
                    .AsNoTracking()
                    .FirstOrDefaultAsync(o => o.ProductOverviewId == id, ct);
                
                if (overview == null)
                {
                    TempData["Error"] = "Không tìm thấy Product Overview.";
                    return RedirectToAction("Index", "Product");
                }

                var productId = overview.ProductId;

                await _delete.HandleAsync(new DeleteProductOverViewInput(id), ct);
                TempData["Success"] = "Xóa Product Overview thành công!";
                return RedirectToAction("ProductDetails", "Product", new { id = productId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi: {ex.Message}";
                return RedirectToAction("Index", "Product");
            }
        }
    }
}
    