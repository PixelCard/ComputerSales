using ComputerSales.Application.UseCase.Category_UC;
using ComputerSales.Application.UseCaseDTO.Category_DTO;
using Microsoft.AspNetCore.Mvc;

namespace ComputerSalesProject_MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class CategoryController : Controller
    {
        private readonly CreateCategory_UC _createCategory;
        private readonly GetByIdCategory_UC _getByIdCategory;
        private readonly UpdateCategory_UC _updateCategory;
        private readonly DeleteCategory_UC _deleteCategory;
        private readonly GetAllCategories_UC _getAllCategories;

        public CategoryController(
            CreateCategory_UC createCategory, 
            GetByIdCategory_UC getByIdCategory, 
            UpdateCategory_UC updateCategory, 
            DeleteCategory_UC deleteCategory,
            GetAllCategories_UC getAllCategories)
        {
            _createCategory = createCategory;
            _getByIdCategory = getByIdCategory;
            _updateCategory = updateCategory;
            _deleteCategory = deleteCategory;
            _getAllCategories = getAllCategories;
        }

        public async Task<IActionResult> Index([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10, [FromQuery] string? q = null, CancellationToken ct = default)
        {
            var model = await _getAllCategories.HandleAsync(
                new CategoryPagedRequest(pageIndex, pageSize, q),
                ct
            );

            return View(model); // Views/Admin/Categories/Index.cshtml (model: CategoryPagedResult)
        }

        [HttpPost("find")]
        public async Task<IActionResult> FindById(long id, CancellationToken ct)
        {
            var found = await _getByIdCategory.HandleAsync(new CategoryOutput(id, string.Empty), ct);
            if (found == null)
            {
                TempData["Error"] = $"Không tìm thấy Category #{id}.";
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Details), new { id = found.id });
        }

        // ========================= DETAILS =========================
        [HttpGet("{id:long}")]
        public async Task<IActionResult> Details(long id, CancellationToken ct)
        {
            var result = await _getByIdCategory.HandleAsync(new CategoryOutput(id, string.Empty), ct);
            if (result == null) return NotFound();
            return View(result); // Views/Admin/Categories/Details.cshtml (model: CategoryOutput)
        }

        // ========================= CREATE =========================
        [HttpGet("create")]
        public IActionResult Create()
        {
            return View(new CategoryInput("")); // model: CategoryInput
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryInput model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);

            var created = await _createCategory.HandleAsync(model, ct);
            if (created == null)
            {
                ModelState.AddModelError("", "Tạo Category thất bại.");
                return View(model);
            }

            TempData["Success"] = $"Đã tạo Category #{created.id}.";
            return RedirectToAction(nameof(Details), new { id = created.id });
        }

        // ========================= EDIT =========================
        [HttpGet("{id:long}/edit")]
        public async Task<IActionResult> Edit(long id, CancellationToken ct)
        {
            var exist = await _getByIdCategory.HandleAsync(new CategoryOutput(id, string.Empty), ct);
            if (exist == null) return NotFound();

            // Form edit dùng CategoryInput cho đơn giản (chỉ sửa name)
            return View(new CategoryInput(exist.name));
        }

        [HttpPost("{id:long}/edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, CategoryInput model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);

            // UC Update đang nhận CategoryOutput (id + name)
            var updated = await _updateCategory.HandleAsync(new CategoryOutput(id, model.name), ct);
            if (updated == null)
            {
                ModelState.AddModelError("", "Cập nhật thất bại hoặc không tìm thấy.");
                return View(model);
            }

            TempData["Success"] = $"Đã cập nhật Category #{updated.id}.";
            return RedirectToAction(nameof(Details), new { id = updated.id });
        }

        // ========================= DELETE =========================
        [HttpGet("{id:long}/delete")]
        public async Task<IActionResult> Delete(long id, CancellationToken ct)
        {
            var exist = await _getByIdCategory.HandleAsync(new CategoryOutput(id, string.Empty), ct);
            if (exist == null) return NotFound();
            return View(exist); // model: CategoryOutput
        }

        [HttpPost("{id:long}/delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(long id, CancellationToken ct)
        {
            var deleted = await _deleteCategory.HandleAsync(new CategoryOutput(id, string.Empty), ct);
            if (deleted == null)
            {
                TempData["Error"] = "Xoá thất bại hoặc không tìm thấy.";
                return RedirectToAction(nameof(Details), new { id });
            }

            TempData["Success"] = $"Đã xoá Category #{id}.";
            return RedirectToAction(nameof(Index));
        }
    }
}
