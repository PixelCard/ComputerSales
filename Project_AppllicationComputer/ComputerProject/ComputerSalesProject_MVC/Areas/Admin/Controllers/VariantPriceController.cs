using ComputerSales.Application.UseCase.VariantPrice_UC;
using ComputerSales.Application.UseCaseDTO.VariantPrice_DTO;
using ComputerSales.Application.UseCaseDTO.VariantPrice_DTO.DeleteVariantPrice;
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

        [HttpGet("Index/{variantId:int}")]
        public async Task<IActionResult> IndexVariantPrice(int variantId, CancellationToken ct)
        {
            // Lấy variant + check có price hay chưa
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
                    HasPrice = v.VariantPrices.Any()   // ✅ check tồn tại giá
                })
                .FirstOrDefaultAsync(ct);

            if (variant == null) return NotFound();

            // lấy giá nếu có
            var price = await _db.variantPrices
                .AsNoTracking()
                .Where(p => p.VariantId == variantId)
                .FirstOrDefaultAsync(ct);

            ViewBag.Price = price;
            return View(variant); // model = ProductVariantDetailVM
        }


        [HttpGet("Create/{variantId:int}")]
        public IActionResult CreateVariantPrice(int variantId)
        {
            ViewBag.StatusList = new SelectList(Enum.GetValues(typeof(PriceStatus)));
            return View(new VariantPriceInputDTO(variantId, "VND", 0, 0, PriceStatus.Active, null, null));
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVariantPrice(VariantPriceInputDTO input, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.StatusList = new SelectList(Enum.GetValues(typeof(PriceStatus)));
                return View(input);
            }

            var rs = await _create.HandleAsync(input, ct);
            if (rs == null) return BadRequest();

            TempData["Success"] = "Thêm giá cho biến thể thành công.";
            return RedirectToAction("IndexVariantPrice", new { variantId = input.VariantId });
        }

        // Update
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
            var rs = await _update.HandleAsync(
                id, // ✅ Id của VariantPrice lấy từ route
                new VariantPriceInputDTO(
                    input.VariantId,
                    input.Currency,
                    input.Price,
                    input.DiscountPrice,
                    input.Status,
                    input.ValidFrom,
                    input.ValidTo
                ),
                ct);

            if (rs == null) return NotFound();

            TempData["Success"] = "Cập nhật giá thành công.";
            return RedirectToAction("IndexVariantPrice", new { variantId = input.VariantId });
        }


        // Delete
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
            var ok = await _delete.HandleAsync(id, ct);
            if (ok == null) return NotFound();

            TempData["Success"] = "Xóa giá thành công.";
            return RedirectToAction("IndexVariantPrice");
        }

    }
}
