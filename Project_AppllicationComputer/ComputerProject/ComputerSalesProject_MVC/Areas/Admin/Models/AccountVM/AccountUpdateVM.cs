using System.ComponentModel.DataAnnotations;

namespace ComputerSalesProject_MVC.Areas.Admin.Models.AccountVM
{
    public sealed class AccountUpdateVM
    {
        [Required] public int IDAccount { get; set; }
        [Required, EmailAddress] public string Email { get; set; } = string.Empty;
        // Nếu không cho sửa Pass, giữ null và không render input trong View
        public string? Pass { get; set; }
        [Range(1, int.MaxValue)] public int IDRole { get; set; }
    }
}
