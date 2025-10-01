using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // AsNoTracking, ToListAsync
using ComputerSales.Infrastructure.Persistence; // AppDbContext

using ComputerSales.Application.UseCase.Account_UC;
using ComputerSales.Application.UseCaseDTO.Account_DTO;
using ComputerSales.Application.UseCaseDTO.Account_DTO.GetAccountByID;
using ComputerSales.Application.UseCaseDTO.Account_DTO.UpdateAccount;
using ComputerSales.Application.UseCaseDTO.Account_DTO.DeleteAccount;
using ComputerSalesProject_MVC.Areas.Admin.Models.AccountVM;

namespace ComputerSalesProject_MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class AccountsController : Controller
    {
        private readonly AppDbContext _db;

        // UseCases: khớp với API của bạn
        private readonly GetAccount_UC _get;
        private readonly UpdateAccount_UC _update;
        private readonly DeleteAccount_UC _delete;
        private readonly CreateAccount_UC _create; // nếu bạn muốn enable Create

        public AccountsController(
            AppDbContext db,
            GetAccount_UC get,
            UpdateAccount_UC update,
            DeleteAccount_UC delete,
            CreateAccount_UC create // nếu chưa DI thì xoá tham số + field và action Create
        )
        {
            _db = db;
            _get = get;
            _update = update;
            _delete = delete;
            _create = create;
        }

     

        // GET: Admin/Accounts/Index
        [HttpGet]
        public async Task<IActionResult>  Index(CancellationToken ct)
        {
            // Lấy toàn bộ account từ DB, map sang AccountOutputDTO để view dùng thống nhất
            var items = await _db.Accounts
                .AsNoTracking()
                .OrderBy(a => a.IDAccount)
                .Select(a => new AccountOutputDTO(
                    a.IDAccount,
                    a.Email,
                    a.IDRole,
                    a.Role != null ? a.Role.TenRole : string.Empty
                ))
                .ToListAsync(ct);

            return View(items); // @model IEnumerable<AccountOutputDTO>
        }

        // ==================== DETAILS (GET BY ID) ====================

        // GET: Admin/Accounts/Details/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var dto = await _get.HandleAsync(new getAccountByID(id), ct);
            if (dto is null) return NotFound();
            return View(dto); // @model AccountOutputDTO
        }

        // ==================== CREATE ====================

        // GET: Admin/Accounts/Create
        [HttpGet]
        public IActionResult Create() => View(new AccountCreateVM());

        // POST: Admin/Accounts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AccountCreateVM vm, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(vm);

            var input = new AccountDTOInput(vm.Email, vm.Pass, vm.IDRole, vm.CreateDate);
            var created = await _create.HandleAsync(input, ct);         // UC trả AccountOutputDTO?

            if (created is null)
            {
                ModelState.AddModelError(string.Empty, "Tạo tài khoản thất bại.");
                return View(vm);
            }

            TempData["Success"] = "Đã tạo tài khoản.";
            return RedirectToAction(nameof(Details), new { id = created.IDAccount });
        }

        // ==================== EDIT / UPDATE ====================

        // GET: Admin/Accounts/Edit/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var dto = await _get.HandleAsync(new getAccountByID(id), ct);
            if (dto is null) return NotFound();

            var vm = new AccountUpdateVM
            {
                IDAccount = dto.IDAccount,
                Email = dto.Email,
                IDRole = dto.IDRole,
                // Pass = null (không bind ngược mật khẩu)
            };
            return View(vm);
        }

        // POST: Admin/Accounts/Edit/5
        [HttpPost("{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AccountUpdateVM vm, CancellationToken ct)
        {
            if (id != vm.IDAccount) return BadRequest("ID không khớp.");
            if (!ModelState.IsValid) return View(vm);

            // Khớp với API: Update nhận (id, UpdateAccountDTO)
            var body = new UpdateAccountDTO(
                vm.Email,
                vm.Pass ?? string.Empty,   // nếu không cho đổi pass, bạn có thể giữ nguyên pass cũ hoặc chặn ở UseCase
                vm.IDRole
            );

            var rs = await _update.HandleAsync(id, body, ct); // API trả Ok(rs) => nhiều khả năng trả AccountOutputDTO
            if (rs is null)
            {
                ModelState.AddModelError(string.Empty, "Cập nhật thất bại.");
                return View(vm);
            }

            TempData["Success"] = "Đã cập nhật tài khoản.";
            return RedirectToAction(nameof(Details), new { id = vm.IDAccount });
        }

        // ==================== DELETE ====================

        // GET: Admin/Accounts/Delete/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var dto = await _get.HandleAsync(new getAccountByID(id), ct);
            if (dto is null) return NotFound();
            return View(dto); // @model AccountOutputDTO
        }

        // POST: Admin/Accounts/Delete/5
        [HttpPost("{id:int}")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            // API Delete: _delete.HandleAsync(new DeleteAccountOutputDTO(id), ct) -> bool
            var ok = await _delete.HandleAsync(new DeleteAccountOutputDTO(id), ct);
            if (!ok)
            {
                TempData["Error"] = "Xóa không thành công.";
                return RedirectToAction(nameof(Details), new { id });
            }

            TempData["Success"] = "Đã xóa tài khoản.";
            return RedirectToAction(nameof(Index));
        }
    }
}
