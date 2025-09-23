using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ComputerSalesProject_MVC.Models
{
    public class RegisterViewModel
    {
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = "";

        public string address { get; set; }

        public string? phone { get; set; } = "";
        public string? Description_User { get; set; }
        public DateTime? Date { get; set; }
        public int? RoleId { get; set; } = 1;
    }
}
