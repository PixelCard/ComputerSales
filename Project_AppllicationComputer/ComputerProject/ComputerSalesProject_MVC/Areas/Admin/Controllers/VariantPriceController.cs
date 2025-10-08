using System.Text;
using ComputerSales.Application.UseCase.VariantPrice_UC;
using ComputerSales.Application.UseCaseDTO.VariantPrice_DTO.VariantPriceInput_Output;
using ComputerSales.Domain.Entity.EVariant;
using ComputerSales.Infrastructure.Persistence;
using ComputerSalesProject_MVC.Areas.Admin.Models.NewFolder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ComputerSalesProject_MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    public class VariantPriceController : Controller
    {
        private readonly AppDbContext _db;
        private readonly CreateVariantPrice_UC _create;
        private readonly UpdateVariantPrice_UC _update;
        private readonly GetByIdVariantPrice_UC _get;
        private readonly DeleteVariantPrice_UC _delete;

        public VariantPriceController(
            AppDbContext db,
            CreateVariantPrice_UC create,
            UpdateVariantPrice_UC update,
            GetByIdVariantPrice_UC get,
            DeleteVariantPrice_UC delete)
        {
            _db = db;
            _create = create;
            _update = update;
            _get = get;
            _delete = delete;
        }

        // ===== INDEX
        [HttpGet("Index/{variantId:int}")]
        public async Task<IActionResult> IndexVariantPrice(int variantId, CancellationToken ct)
        {
            var variant = await _db.productVariants
                .AsNoTracking()
                .Where(v => v.Id == variantId)
                .Select(v => new ProductVariantDetailVM
                {
                    Id = v.Id,
                    ProductId = v.ProductId,
                    SKU = v.SKU,
                    VariantName = v.VariantName,
                    Quantity = v.Quantity,
                    Status = v.Status,
                    HasPrice = v.VariantPrices.Any()
                })
                .FirstOrDefaultAsync(ct);

            if (variant == null) return NotFound();

            var price = await _db.variantPrices
                .AsNoTracking()
                .Where(p => p.VariantId == variantId)
                .OrderByDescending(p => p.Id)
                .FirstOrDefaultAsync(ct);

            ViewBag.Price = price;
            return View(variant);
        }

        // ===== CREATE (GET) -> /Admin/VariantPrice/CreateVariantPrice/2
        [HttpGet("CreateVariantPrice/{variantId:int}")]
        public IActionResult CreateVariantPrice(int variantId)
        {
            ViewBag.StatusList = new SelectList(Enum.GetValues(typeof(PriceStatus)));
            return View(new VariantPriceInputDTO(variantId, "VND", 0, 0, PriceStatus.Active, null, null));
            // View file phải tên: Areas/Admin/Views/VariantPrice/CreateVariantPrice.cshtml
        }

        // ===== CREATE (POST) -> /Admin/VariantPrice/CreateVariantPrice
        [HttpPost("CreateVariantPrice")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVariantPrice(VariantPriceInputDTO input, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.StatusList = new SelectList(Enum.GetValues(typeof(PriceStatus)));
                TempData["Error"] = "⚠️ Dữ liệu không hợp lệ.";
                return View(input);
            }

            // Ràng buộc giá
            if (input.Price <= 1 || input.Price >= 50_000_000)
            {
                ModelState.AddModelError("Price", "⚠️ Giá gốc phải lớn hơn 1 và nhỏ hơn 50,000,000.");
                ViewBag.StatusList = new SelectList(Enum.GetValues(typeof(PriceStatus)));
                return View(input);
            }
            if (input.DiscountPrice < 0)
            {
                ModelState.AddModelError("DiscountPrice", "⚠️ Giá giảm không được âm.");
                ViewBag.StatusList = new SelectList(Enum.GetValues(typeof(PriceStatus)));
                return View(input);
            }
            if (input.DiscountPrice > input.Price)
                input = input with { DiscountPrice = 0 };

            // Hết hạn khuyến mãi -> chỉ áp dụng giá gốc
            if (input.ValidTo.HasValue && DateTime.Now > input.ValidTo.Value)
            {
                input = input with { DiscountPrice = 0 };
                TempData["Warning"] = "⚠️ Khuyến mãi đã hết hạn, chỉ áp dụng giá gốc.";
            }

            var finalPrice = Math.Max(0, input.Price - input.DiscountPrice);

            try
            {
                var rs = await _create.HandleAsync(input, ct);
                if (rs == null)
                {
                    TempData["Error"] = "❌ Không tạo được bản ghi.";
                    ViewBag.StatusList = new SelectList(Enum.GetValues(typeof(PriceStatus)));
                    return View(input);
                }

                TempData["Success"] = $"✅ Thêm giá thành công. Giá bán thực tế: {finalPrice:N0}₫";
                return RedirectToAction("IndexVariantPrice", new { variantId = input.VariantId });
            }
            catch (DbUpdateException ex)
            {
                TempData["Error"] = "❌ Lỗi CSDL: " + (ex.InnerException?.Message ?? ex.Message);
                ViewBag.StatusList = new SelectList(Enum.GetValues(typeof(PriceStatus)));
                return View(input);
            }
        }

        // ===== UPDATE
        [HttpGet("Update/{id:int}")]
        public async Task<IActionResult> UpdateVariantPrice(int id, CancellationToken ct)
        {
            var rs = await _get.HandleAsync(id, ct);
            if (rs == null) return NotFound();
            ViewBag.StatusList = new SelectList(Enum.GetValues(typeof(PriceStatus)));
            return View(rs);
        }

        [HttpPost("Update/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVariantPrice(int id, VariantPriceOutputDTO input, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.StatusList = new SelectList(Enum.GetValues(typeof(PriceStatus)));
                TempData["Error"] = "⚠️ Dữ liệu không hợp lệ.";
                return View(input);
            }
            if (input.Price <= 1 || input.Price >= 50_000_000)
            {
                ModelState.AddModelError("Price", "⚠️ Giá gốc phải lớn hơn 1 và nhỏ hơn 50,000,000.");
                ViewBag.StatusList = new SelectList(Enum.GetValues(typeof(PriceStatus)));
                return View(input);
            }
            if (input.DiscountPrice < 0)
            {
                ModelState.AddModelError("DiscountPrice", "⚠️ Giá giảm không được âm.");
                ViewBag.StatusList = new SelectList(Enum.GetValues(typeof(PriceStatus)));
                return View(input);
            }
            if (input.DiscountPrice > input.Price)
                input = input with { DiscountPrice = 0 };

            if (input.ValidTo.HasValue && DateTime.Now > input.ValidTo.Value)
            {
                input = input with { DiscountPrice = 0 };
                TempData["Warning"] = "⚠️ Giá khuyến mãi đã hết hạn, chỉ giữ lại giá gốc.";
            }

            var finalPrice = Math.Max(0, input.Price - input.DiscountPrice);

            var rs = await _update.HandleAsync(
                id,
                new VariantPriceInputDTO(
                    input.VariantId, input.Currency, input.Price,
                    input.DiscountPrice, input.Status, input.ValidFrom, input.ValidTo),
                ct);

            if (rs == null) return NotFound();

            TempData["Success"] = $"✅ Cập nhật giá thành công. Giá bán thực tế: {finalPrice:N0}₫";
            return RedirectToAction("IndexVariantPrice", new { variantId = input.VariantId });
        }

        // ===== DELETE
        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> DeleteVariantPrice(int id, CancellationToken ct)
        {
            var rs = await _get.HandleAsync(id, ct);
            if (rs == null) return NotFound();
            return View(rs);
        }

        [HttpPost("Delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var price = await _db.variantPrices.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, ct);
            var variantId = price?.VariantId ?? 0;

            var ok = await _delete.HandleAsync(id, ct);
            if (ok == null) return NotFound();

            TempData["Success"] = "🗑️ Xóa giá thành công.";
            return RedirectToAction("IndexVariantPrice", new { variantId });
        }
    }
}
