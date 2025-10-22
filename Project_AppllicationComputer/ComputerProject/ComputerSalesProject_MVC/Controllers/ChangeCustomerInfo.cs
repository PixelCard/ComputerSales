using ComputerSales.Application.UseCase.Customer_UC;
using ComputerSales.Application.UseCaseDTO.Customer_DTO.getCustomerByUserID;
using ComputerSales.Application.UseCaseDTO.Customer_DTO.UpdateCustomerDTO;
using ComputerSalesProject_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ComputerSalesProject_MVC.Controllers
{
    public class ChangeCustomerInfo : Controller
    {
        private readonly getCustomerByID _getById;
        private readonly IWebHostEnvironment _env;
        private readonly getCustomerByUserID _getByUserId;
        private readonly UpdateCustomer_UC _update; 
        public ChangeCustomerInfo(getCustomerByID getById, getCustomerByUserID getByUserId, UpdateCustomer_UC update, IWebHostEnvironment env)
        {
            _getById = getById;
            _getByUserId = getByUserId;
            _update = update;
            _env = env;
        }
        [HttpGet]
        public async Task<IActionResult> EditByUser(CancellationToken ct)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userID) || userID <= 0)
                return RedirectToAction("Login", "Account");

            var dto = await _getByUserId.HandleAsync(
                new CustomerGetCustomerByUserID_Request(userID), ct);

            if (dto is null) return NotFound();

            var vm = new CustomerEditVM_Customer
            {
                IDCustomer = dto.IDCustomer,
                IDAccount = userID,
                IMG = dto.IMG,
                Name = dto.Name,
                Description = dto.Description,
                sdt = dto.sdt,
                address = dto.address,
                Date = dto.Date
            };
            return View("EditByUser", vm); // reuse view Edit
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditByUser(CustomerEditVM_Customer vm, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(vm);


            if (vm.AvatarFile is { Length: > 0 })
            {
                // Giới hạn 2MB (tùy anh)
                const long MAX = 2 * 1024 * 1024;
                if (vm.AvatarFile.Length > MAX)
                {
                    TempData["Error"] = "Ảnh quá lớn (tối đa 2MB).";
                    return View(vm);
                }

                // Kiểm MIME chính thống
                var allowedMime = new[] { "image/jpeg", "image/png", "image/webp" };

                if (!allowedMime.Contains(vm.AvatarFile.ContentType))
                {
                    TempData["Error"] = "Định dạng ảnh không hỗ trợ (chỉ JPG/PNG/WebP).";
                    return View(vm);
                }

                // Kiểm magic bytes (anti spoof)
                if (!await IsValidImageSignatureAsync(vm.AvatarFile))
                {
                    TempData["Error"] = "File ảnh không hợp lệ.";
                    return View(vm);
                }

                // Tạo thư mục: wwwroot/uploads/customers/yyyy/MM
                var targetDir = Path.Combine(_env.WebRootPath, "img");
                Directory.CreateDirectory(targetDir);

                // Tên file random theo MIME
                string ext = vm.AvatarFile.ContentType switch
                {
                    "image/jpeg" => ".jpg",
                    "image/png" => ".png",
                    "image/webp" => ".webp",
                    _ => ".bin"
                };

                var fileName = $"{Guid.NewGuid():N}{ext}";

                var absPath = Path.Combine(targetDir, fileName);

                using (var stream = System.IO.File.Create(absPath))
                    await vm.AvatarFile.CopyToAsync(stream, ct);

                // Lưu đường dẫn tương đối vào DB
                vm.IMG = $"/img/{fileName}";
            }
            else
            {
                // Nếu không có file, có thể kiểm tra IMG (URL) là internal
                if (!string.IsNullOrWhiteSpace(vm.IMG) && !vm.IMG.StartsWith("/uploads/", StringComparison.OrdinalIgnoreCase))
                {
                    TempData["Error"] = "Chỉ chấp nhận ảnh nội bộ đã upload.";
                    return View(vm);
                }
            }

            // 2) Map sang DTO update và lưu
            var input = new InputUpdateCustomerDTO(vm.IMG, vm.Name, vm.Description, vm.sdt, vm.address, vm.Date);

            var updated = await _update.HandleAsync(vm.IDCustomer, input, ct);
            if (updated is null)
            {
                TempData["Error"] = "Không tìm thấy khách hàng.";
                return View(vm);
            }

            TempData["Success"] = "Cập nhật thông tin khách hàng thành công.";
            return RedirectToAction(nameof(EditByUser), new { id = vm.IDCustomer });
        }

        // Kiểm tra magic bytes: JPG/PNG/WebP
        private static async Task<bool> IsValidImageSignatureAsync(IFormFile file)
        {
            // đọc tối đa 16 byte đầu
            byte[] head = new byte[Math.Min(16, file.Length)];
            using (var s = file.OpenReadStream())
            {
                _ = await s.ReadAsync(head, 0, head.Length);
            }

            // JPEG: FF D8 FF
            if (head.Length >= 3 && head[0] == 0xFF && head[1] == 0xD8 && head[2] == 0xFF) return true;

            // PNG: 89 50 4E 47 0D 0A 1A 0A
            if (head.Length >= 8 &&
                head[0] == 0x89 && head[1] == 0x50 && head[2] == 0x4E && head[3] == 0x47 &&
                head[4] == 0x0D && head[5] == 0x0A && head[6] == 0x1A && head[7] == 0x0A) return true;

            // WebP: "RIFF" .... "WEBP"
            if (head.Length >= 12 &&
                head[0] == 0x52 && head[1] == 0x49 && head[2] == 0x46 && head[3] == 0x46 &&
                head[8] == 0x57 && head[9] == 0x45 && head[10] == 0x42 && head[11] == 0x50) return true;

            return false;
        }
    }
}
