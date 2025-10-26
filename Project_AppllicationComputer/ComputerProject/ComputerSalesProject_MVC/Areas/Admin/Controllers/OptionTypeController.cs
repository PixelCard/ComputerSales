using ComputerSales.Application.UseCase.OptionalValue_UC;
using ComputerSales.Application.UseCaseDTO.OptionalValue_DTO;
using ComputerSales.Application.UseCaseDTO.OptionalValue_DTO.DeleteOptionalValue_DTO;
using ComputerSales.Application.UseCaseDTO.OptionalValue_DTO.GetByIdOptionalValue_DTO;
using ComputerSales.Application.UseCaseDTO.OptionalValue_DTO.UpdateOptionalValue_DTO;
using ComputerSales.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ComputerSalesProject_MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class OptionValueController : Controller
    {
        private readonly AppDbContext _db;
        private readonly CreateOptionalValue_UC _create;
        private readonly GetByIdOptionalValue_UC _get;
        private readonly UpdateOptionalValue_UC _update;
        private readonly DeleteOptionalValue_UC _delete;

        public OptionValueController(
            AppDbContext db,
            CreateOptionalValue_UC create,
            GetByIdOptionalValue_UC get,
            UpdateOptionalValue_UC update,
            DeleteOptionalValue_UC delete)
        {
            _db = db;
            _create = create;
            _get = get;
            _update = update;
            _delete = delete;
        }

        private void LoadOptionTypes()
        {
            ViewBag.OptionTypes = new SelectList(
                _db.optionTypes.AsNoTracking().ToList(),
                "Id", "Name"
            );
        }

        // Danh sách OptionValue
        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var values = await _db.optionalValues
                .Include(v => v.OptionType)
                .Select(v => new OptionalValueOutput(
                    v.Id,
                    v.OptionTypeId,
                    
                    v.Value,
                    v.SortOrder,
                    v.Price
                ))
                .ToListAsync(ct);

            return View(values);
        }

        // Tạo mới
        [HttpGet]
        public IActionResult Create()
        {
            LoadOptionTypes();
            return View(new OptionalValueInput(0, "", 0, 0));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OptionalValueInput input, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                LoadOptionTypes();
                return View(input);
            }

            await _create.HandleAsync(input, ct);
            TempData["Success"] = "Thêm OptionValue thành công!";
            return RedirectToAction(nameof(Index));
        }

        // Sửa
        [HttpGet("{id:long}")]
        public async Task<IActionResult> Edit(long id, CancellationToken ct)
        {
            var rs = await _get.HandleAsync(new GetByIdOptionalValueInput((int)id), ct);
            if (rs == null) return NotFound();

            LoadOptionTypes();
            return View(new UpdateOptionalValueInput(
                rs.Id,
                rs.OptionTypeId,
                rs.Value,
                rs.SortOrder,
                rs.Price
            ));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateOptionalValueInput input, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                LoadOptionTypes();
                return View(input);
            }

            await _update.HandleAsync(input, ct);
            TempData["Success"] = "Cập nhật OptionValue thành công!";
            return RedirectToAction(nameof(Index));
        }

        // Xóa
        [HttpGet("{id:long}")]
        public async Task<IActionResult> Delete(long id, CancellationToken ct)
            {
                var rs = await _get.HandleAsync(new GetByIdOptionalValueInput((int)id), ct);
            if (rs == null) return NotFound();

            return View(rs);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id, CancellationToken ct)
        {
            await _delete.HandleAsync(new DeleteOptionalValueInput((int)id), ct);
            TempData["Success"] = "Xóa OptionValue thành công!";
            return RedirectToAction(nameof(Index));
        }
    }
}
