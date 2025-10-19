using System.ComponentModel.DataAnnotations;

namespace ComputerSalesProject_MVC.Models
{
    public sealed class LoginViewModel
    {
        [Required(ErrorMessage = "Email là bắt buộc")]
        [MaxLength(255,ErrorMessage = "Không được vượt quá 255 kí tự")]
        public string email { get; set; } = default!;

        [Required(ErrorMessage = "Pass là bắt buộc")]
        public string pass { get; set; } = "";
    }
}
