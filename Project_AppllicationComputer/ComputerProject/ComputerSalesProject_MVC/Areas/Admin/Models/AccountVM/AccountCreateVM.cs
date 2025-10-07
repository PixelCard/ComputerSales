using System.ComponentModel.DataAnnotations;

namespace ComputerSalesProject_MVC.Areas.Admin.Models.AccountVM
{
    public sealed class AccountCreateVM
    {
        [Required, EmailAddress] public string Email { get; set; } = string.Empty;
        [Required] public string Pass { get; set; } = string.Empty;
        [Range(1, int.MaxValue)] public int IDRole { get; set; }
        [DataType(DataType.Date)] public DateTime CreateDate { get; set; } = DateTime.Today;
    }
}
