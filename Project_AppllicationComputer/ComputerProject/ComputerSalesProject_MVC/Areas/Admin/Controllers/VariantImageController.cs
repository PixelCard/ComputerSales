using ComputerSales.Application.UseCase.VariantImage_UC;
using ComputerSales.Application.UseCaseDTO.NewFolder;
using ComputerSales.Application.UseCaseDTO.VariantImage;
using ComputerSales.Application.UseCaseDTO.VariantImage.DeleteVariantImage;
using ComputerSales.Application.UseCaseDTO.VariantImageDTO;
using ComputerSales.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComputerSalesProject_MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class VariantImageController : Controller
    {
        private readonly AppDbContext _db;
        private readonly CreateVariantImage_UC _create;
        private readonly getVariantImageById_UC _get;
        private readonly UpdateVariantImage_UC _update;
        private readonly DeleteVariantImage_UC _delete;

        public VariantImageController(
            AppDbContext db,
            CreateVariantImage_UC create,
            getVariantImageById_UC get,
            UpdateVariantImage_UC update,
            DeleteVariantImage_UC delete)
        {
            _db = db;
            _create = create;
            _get = get;
            _update = update;
            _delete = delete;
        }

        // Danh sách ảnh của 1 Variant
        [HttpGet]
        
        public async Task<IActionResult> IndexVariantImage(int variantId, CancellationToken ct)
        {
            var images = await _db.variantImages
                .Where(x => x.VariantId == variantId)
                .OrderBy(x => x.SortOrder)
                .ToListAsync(ct);

            // ⚡ Map sang DTO
            var vm = images.Select(x => x.ToResult()).ToList();

            ViewBag.VariantId = variantId;
            return View(vm); // ✅ Truyền DTO, đúng với @model trong View
        }


        // Create
        [HttpGet]
        public IActionResult CreateVariantImage(int variantId)
        {
            return View(new VariantImageInputDTO(variantId, "", 0, ""));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVariantImage(VariantImageInputDTO input, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(input);

            // ✅ Auto SortOrder
            var maxSort = await _db.variantImages
                .Where(x => x.VariantId == input.VariantId)
                .MaxAsync(x => (int?)x.SortOrder, ct) ?? 0;

            input = input with { SortOrder = maxSort + 1 };

            var rs = await _create.HandleAsync(input, ct);
            if (rs == null) return BadRequest();

            TempData["Success"] = "Thêm ảnh biến thể thành công.";
            return RedirectToAction("IndexVariantImage", new { variantId = input.VariantId });
        }


        // Update
        [HttpGet("{id:int}")]
        public async Task<IActionResult> UpdateVariantImage(int id, CancellationToken ct)
        {
            var rs = await _get.HandleAsync(id, ct);
            if (rs == null) return NotFound();

            return View(rs);
        }

        [HttpPost("{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVariantImage(int id, VariantImageOutputDTO input, CancellationToken ct)
        {
            if (id != input.Id) return BadRequest("ID không khớp.");

            var rs = await _update.HandleAsync(input, ct);
            if (rs == null) return NotFound();

            TempData["Success"] = "Cập nhật ảnh biến thể thành công.";
            return RedirectToAction("IndexVariantImage", new { variantId = input.VariantId });
        }

        // Delete
        [HttpGet("{id:int}")]
        public async Task<IActionResult> DeleteVariantImage(int id, CancellationToken ct)
        {
            var rs = await _get.HandleAsync(id, ct);
            if (rs == null) return NotFound();

            return View(rs);
        }

        [HttpPost("{id:int}"), ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var ok = await _delete.HandleAsync(new DeleteVariantImageInput(id), ct);
            if (!ok) return NotFound();

            TempData["Success"] = "Xóa ảnh biến thể thành công.";
            return RedirectToAction("IndexVariantImage"); // có thể cần variantId nếu muốn quay về danh sách
        }
    }
}
