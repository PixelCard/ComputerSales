using ComputerSales.Application.UseCase.Customer_UC;
using ComputerSales.Application.UseCaseDTO.Customer_DTO;
using ComputerSales.Application.UseCaseDTO.Customer_DTO.getCustomerByID;
using ComputerSales.Infrastructure.Persistence; // AppDbContext
using ComputerSalesProject_MVC.Areas.Admin.Models.CustomerVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // AsNoTracking(), ToListAsync()
using System.ComponentModel.DataAnnotations;

namespace ComputerSalesProject_MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]    
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class CustomersController : Controller
    {
        private readonly AppDbContext _db;
        private readonly CreateCustomer_UC _create;
        private readonly getCustomerByID _getById;
        private readonly DeleteCustomer_UC _delete;

        public CustomersController(
            AppDbContext db,
            CreateCustomer_UC create,
            getCustomerByID getById,
            DeleteCustomer_UC delete)
        {
            _db = db;
            _create = create;
            _getById = getById;
            _delete = delete;
        }
       
        // GET: Admin/Customers/Index — hiển thị toàn bộ Customer trong DB
        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var items = await _db.Customers
                .AsNoTracking()
                .OrderBy(c => c.CustomerID)
                .Select(c => new CustomerOutputDTO(
                    c.CustomerID,
                    c.IMG,
                    c.Name,
                    c.Description,
                    c.sdt,
                    c.address,
                    c.Date,
                    c.IDAccount
                ))
                .ToListAsync(ct);

            return View(items); // View model: IEnumerable<CustomerOutputDTO>
        }

        // GET: Admin/Customers/Create
        [HttpGet]
        public IActionResult Create() => View(new CustomerCreateVM());

        // POST: Admin/Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerCreateVM vm, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(vm);

            var input = new CustomerInputDTO(
                vm.IMG,
                vm.Name,
                vm.Description,
                vm.address,
                vm.sdt,
                vm.Date,
                vm.IDAccount
            );

            var created = await _create.HandleAsync(input, ct);
            if (created is null)
            {
                ModelState.AddModelError(string.Empty, "Tạo khách hàng thất bại.");
                return View(vm);
            }

            TempData["Success"] = "Tạo khách hàng thành công.";
            return RedirectToAction(nameof(Details), new { id = created.IDCustomer });
        }

        // GET: Admin/Customers/Details/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var dto = await _getById.HandleAsync(new InputGetCustomerByID(id), ct);
            if (dto is null) return NotFound();
            return View(dto); // View model: OutputGetCustomerByID (hoặc kiểu bạn trả về)
        }

        // GET: Admin/Customers/Delete/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var dto = await _getById.HandleAsync(new InputGetCustomerByID(id), ct);
            if (dto is null) return NotFound();
            return View(dto);
        }

        // POST: Admin/Customers/Delete/5
        [HttpPost("{id:int}")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            // 1) Lấy lại bản ghi theo id
            var found = await _getById.HandleAsync(new InputGetCustomerByID(id), ct);
            if (found is null)
            {
                TempData["Error"] = "Không tìm thấy khách hàng.";
                return RedirectToAction(nameof(Index));
            }

            // 2) Map sang đúng kiểu mà DeleteCustomer_UC đang yêu cầu: CustomerOutputDTO
            var dto = new CustomerOutputDTO(
                found.IDCustomer,
                found.IMG,
                found.Name,
                found.Description,
                found.sdt,
                found.address,
                found.Date,
                found.IDAccount
            );

            // 3) Gọi UC xoá
            var deleted = await _delete.HandleAsync(dto, ct);

            // Nếu UC trả bool, đổi điều kiện thành: if (!deleted) { ... }
            if (deleted is null)
            {
                TempData["Error"] = "Xóa không thành công.";
                return RedirectToAction(nameof(Details), new { id });
            }

            TempData["Success"] = "Đã xóa khách hàng.";
            return RedirectToAction(nameof(Index));
        }
        // GET: Admin/Customers/Edit/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> EditCustomer(int id, CancellationToken ct)
        {
            var c = await _db.Customers.AsNoTracking().FirstOrDefaultAsync(x => x.CustomerID == id, ct);
            if (c is null) return NotFound();

            var vm = new CustomerEditVM
            {
                IDCustomer = c.CustomerID,
                IDAccount = c.IDAccount,
                IMG = c.IMG,
                Name = c.Name,
                Description = c.Description,
                sdt = c.sdt,
                address = c.address,
                Date = c.Date
            };
            return View(vm);
        }

        // POST: Admin/Customers/Edit/5
        [HttpPost("{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCustomer(int id, CustomerEditVM vm, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(vm);

            var entity = await _db.Customers.FirstOrDefaultAsync(x => x.CustomerID == id, ct);
            if (entity is null) return NotFound();

            // chỉ cập nhật field cho phép
            entity.IMG = vm.IMG;
            entity.Name = vm.Name;
            entity.Description = vm.Description;
            entity.sdt = vm.sdt;
            entity.address = vm.address;
            entity.Date = vm.Date;

            await _db.SaveChangesAsync(ct);

            TempData["Success"] = "Đã lưu thông tin khách hàng.";
            // quay về trang chi tiết account kèm customer này
            return RedirectToAction("Details", "Accounts", new { area = "Admin", id = entity.IDAccount });
        }

    }
}
