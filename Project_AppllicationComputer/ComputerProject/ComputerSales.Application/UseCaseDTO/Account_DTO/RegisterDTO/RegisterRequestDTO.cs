using System.ComponentModel.DataAnnotations;

namespace ComputerSales.Application.UseCaseDTO.Account_DTO.RegisterDTO
{
    public class RegisterRequestDTO
    {
        [Required(ErrorMessage = "Tên người dùng là bắt buộc")]
        [StringLength(50,ErrorMessage ="Tên người dùng tối đa 50 kí tự")]
        public string UserName { get; set; } = default!;

        [Required(ErrorMessage = "Email bắt buộc phải có")]
        public string Email { get; set; } = default!;

        [Required(ErrorMessage = "Password bắt buộc phải có")]
        public string Password { get; set; } = "";

        public string address { get; set; }

        public string? phone { get; set; } = "";
        public string? Description_User { get; set; }
        public DateTime? Date { get; set; }
        public int? RoleId { get; set; } = 1;
    }
}
