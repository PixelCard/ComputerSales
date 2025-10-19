using ComputerSales.Application.UseCase.AccountBlock_UC;
using ComputerSales.Application.UseCaseDTO.AccountBlock_DTO;
using ComputerSales.Application.UseCaseDTO.AccountBlock_DTO.DeleteAccountBlock;
using ComputerSales.Application.UseCaseDTO.AccountBlock_DTO.GetAccountBlock;
using ComputerSales.Infrastructure.Persistence;
using ComputerSalesProject_MVC.Areas.Admin.Models.AccountBlocks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComputerSalesProject_MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class AccountBlocksController : Controller
    {
        private readonly AppDbContext _db;

        private readonly GetAllAccountBlock_UC _getAll;
        private readonly GetAccountBlockById_UC _getById;
        private readonly CreateAccountBlock_UC _create;
        private readonly DeleteAccountBlock_UC _delete;
        private readonly CheckAccountBlock_UC _checkActive;

        public AccountBlocksController(
            AppDbContext db,
            GetAllAccountBlock_UC getAll,
            GetAccountBlockById_UC getById,
            CreateAccountBlock_UC create,
            DeleteAccountBlock_UC delete,
            CheckAccountBlock_UC checkActive
        )
        {
            _db = db;
            _getAll = getAll;
            _getById = getById;
            _create = create;
            _delete = delete;
            _checkActive = checkActive;
        }

        // Múi giờ Việt Nam (Windows)
        private static TimeZoneInfo VnTz =>
            TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

        // ==================== INDEX ====================
        [HttpGet]
        public async Task<IActionResult> IndexAccountBlocks(CancellationToken ct)
        {
            var items = await _getAll.HandleAsync(ct);

            // helper convert UTC -> VN cho View
            ViewBag.ToVn = (Func<DateTime, string>)(utc =>
                TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(utc, DateTimeKind.Utc), VnTz)
                             .ToString("yyyy-MM-dd HH:mm:ss"));

            return View(items); // @model IEnumerable<AccountBlockOutputDTO>
        }

        // ==================== DETAILS ====================
        [HttpGet("{id:int}")]
        public async Task<IActionResult> AccountBlocksDetails(int id, CancellationToken ct)
        {
            var dto = await _getById.HandleAsync(new GetAccountBlockByID_InputDTO(id), ct);
            if (dto is null) return NotFound();

            var fromVN = TimeZoneInfo.ConvertTimeFromUtc(
                DateTime.SpecifyKind(dto.BlockFromUtc, DateTimeKind.Utc), VnTz);

            ViewBag.FromVN = fromVN.ToString("yyyy-MM-dd HH:mm:ss");

            ViewBag.ToVN = dto.BlockToUtc.HasValue
                ? TimeZoneInfo.ConvertTimeFromUtc(
                      DateTime.SpecifyKind(dto.BlockToUtc.Value, DateTimeKind.Utc), VnTz)
                  .ToString("yyyy-MM-dd HH:mm:ss")
                : "—";

            return View(dto); // @model AccountBlockOutputDTO
        }

        // ==================== CREATE ====================
        [HttpGet]
        public IActionResult CreateAccountBlocks()
        {
            // Gợi ý giờ mặc định: từ bây giờ VN, và +1 giờ
            var nowVn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, VnTz);
            var vm = new AccountBlockCreateVM
            {
                BlockFromUtc = nowVn,
                BlockToUtc = nowVn.AddHours(1)
            };

            // Gửi string format sẵn xuống input (không cần DisplayFormat)
            ViewBag.BlockFromForInput = vm.BlockFromUtc.ToString("yyyy-MM-ddTHH:mm:ss");
            ViewBag.BlockToForInput = vm.BlockToUtc?.ToString("yyyy-MM-ddTHH:mm:ss");

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAccountBlocks(AccountBlockCreateVM vm, CancellationToken ct)
        {
            // ====== VALIDATE THỦ CÔNG (không dùng DataAnnotations) ======
            if (vm.IDAccount <= 0)
                ModelState.AddModelError(nameof(vm.IDAccount), "Vui lòng nhập Account ID hợp lệ.");

            if (vm.BlockFromUtc == default)
                ModelState.AddModelError(nameof(vm.BlockFromUtc), "Vui lòng chọn thời gian bắt đầu.");

            if (!string.IsNullOrEmpty(vm.ReasonBlock) && vm.ReasonBlock.Length > 500)
                ModelState.AddModelError(nameof(vm.ReasonBlock), "Lý do tối đa 500 ký tự.");

            // Chuyển input giờ Việt Nam -> UTC
            var fromUtc = TimeZoneInfo.ConvertTimeToUtc(
                DateTime.SpecifyKind(vm.BlockFromUtc, DateTimeKind.Unspecified), VnTz);

            DateTime? toUtc = vm.BlockToUtc.HasValue
                ? TimeZoneInfo.ConvertTimeToUtc(
                    DateTime.SpecifyKind(vm.BlockToUtc.Value, DateTimeKind.Unspecified), VnTz)
                : null;

            // So sánh from/to (trên UTC)
            if (toUtc.HasValue && toUtc.Value <= fromUtc)
                ModelState.AddModelError(nameof(vm.BlockToUtc), "Thời gian kết thúc phải lớn hơn thời gian bắt đầu.");

            // Account tồn tại?
            var exists = await _db.Accounts.AsNoTracking()
                .AnyAsync(a => a.IDAccount == vm.IDAccount, ct);
            if (!exists)
                ModelState.AddModelError(nameof(vm.IDAccount), "Tài khoản không tồn tại.");

            // Overlap (trên UTC)
            if (ModelState.ErrorCount == 0)
            {
                var overlap = await _db.AccountBlocks.AsNoTracking().AnyAsync(b =>
                        b.IDAccount == vm.IDAccount &&
                        fromUtc < (b.BlockToUtc ?? DateTime.MaxValue) &&
                        (toUtc ?? DateTime.MaxValue) > b.BlockFromUtc,
                    ct);

                if (overlap)
                    ModelState.AddModelError(string.Empty, "Khoảng thời gian này bị trùng với một block khác.");
            }

            // Nếu fail, trả về view với giá trị chuỗi cho input datetime-local
            if (!ModelState.IsValid)
            {
                ViewBag.BlockFromForInput = vm.BlockFromUtc.ToString("yyyy-MM-ddTHH:mm:ss");
                ViewBag.BlockToForInput = vm.BlockToUtc?.ToString("yyyy-MM-ddTHH:mm:ss");
                return View(vm);
            }

            // ====== LƯU ======
            var input = new AccountBlockInputDTO(
                vm.IDAccount,
                fromUtc,
                toUtc,
                vm.ReasonBlock?.Trim()
            );

            var created = await _create.HandleAsync(input, ct);
            TempData["Success"] = created.IsActiveNowUtc
                ? "Đã tạo block và đang hiệu lực."
                : "Đã tạo block (chưa hiệu lực tại thời điểm hiện tại).";

            return RedirectToAction(nameof(AccountBlocksDetails), new { id = created.BlockId });
        }

        // ==================== DELETE ====================
        [HttpGet("{id:int}")]
        public async Task<IActionResult> DeleteAccountBlocks(int id, CancellationToken ct)
        {
            var dto = await _getById.HandleAsync(new GetAccountBlockByID_InputDTO(id), ct);
            if (dto is null) return NotFound();

            var fromVN = TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(dto.BlockFromUtc, DateTimeKind.Utc), VnTz);
            ViewBag.FromVN = fromVN.ToString("yyyy-MM-dd HH:mm:ss");

            ViewBag.ToVN = dto.BlockToUtc.HasValue
                ? TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(dto.BlockToUtc.Value, DateTimeKind.Utc), VnTz)
                  .ToString("yyyy-MM-dd HH:mm:ss")
                : "—";

            return View(dto); // @model AccountBlockOutputDTO
        }

        [HttpPost("{id:int}")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var ok = await _delete.HandleAsync(new DeleteAccountBlockInput_DTO(id), ct);
            if (!ok)
            {
                TempData["Error"] = "Xóa không thành công.";
                return RedirectToAction(nameof(AccountBlocksDetails), new { id });
            }
            
            TempData["Success"] = "Đã xóa block.";
            return RedirectToAction(nameof(IndexAccountBlocks));
        }

        // ==================== CHECK ACTIVE ====================
        [HttpGet("{accountId:int}")]
        public async Task<IActionResult> CheckActive(int accountId, CancellationToken ct)
        {
            var active = await _checkActive.HandleAsync(accountId, ct);
            if (active == null)
            {
                return Json(new { accountId, isBlocked = false });
            }
            return Json(new
            {
                accountId,
                isBlocked = true,
                block = active
            });
        }
    }
}
