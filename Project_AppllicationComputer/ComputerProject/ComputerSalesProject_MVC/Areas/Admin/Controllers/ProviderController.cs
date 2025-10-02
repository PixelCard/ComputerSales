using Microsoft.AspNetCore.Mvc;
using ComputerSales.Application.UseCase.Provider_UC;
using ComputerSales.Application.UseCaseDTO.Provider_DTO;

namespace ComputerSalesProject_MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class ProviderController : Controller
    {
        private readonly GetAllProviders_UC _getAll;
        private readonly CreateProvider_UC _create;
        private readonly UpdateProvider_UC _update;
        private readonly DeleteProvider_UC _delete;
        private readonly GetByIdProvider_UC _getById;

        public ProviderController(
            GetAllProviders_UC getAll,
            CreateProvider_UC create,
            UpdateProvider_UC update,
            DeleteProvider_UC delete,
            GetByIdProvider_UC getById)
        {
            _getAll = getAll;
            _create = create;
            _update = update;
            _delete = delete;
            _getById = getById;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var providers = await _getAll.HandleAsync(ct);
            return View(providers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProviderInput input, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(input);
            var created = await _create.HandleAsync(input, ct);
            if (created == null)
            {
                TempData["ErrorMessage"] = "Tạo nhà cung cấp thất bại";
                return View(input);
            }
            TempData["SuccessMessage"] = "Tạo thành công";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long id, CancellationToken ct)
        {
            var found = await _getById.HandleAsync(new ProviderOutput(id, string.Empty), ct);
            if (found == null) return RedirectToAction(nameof(Index));
            return View(found);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProviderOutput input, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(input);
            var updated = await _update.HandleAsync(input, ct);
            if (updated == null)
            {
                TempData["ErrorMessage"] = "Cập nhật thất bại";
                return View(input);
            }
            TempData["SuccessMessage"] = "Cập nhật thành công";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(long id, CancellationToken ct)
        {
            var deleted = await _delete.HandleAsync(new ProviderOutput(id, string.Empty), ct);
            TempData["SuccessMessage"] = deleted != null ? "Xóa thành công" : "Xóa thất bại";
            return RedirectToAction(nameof(Index));
        }
    }
}
