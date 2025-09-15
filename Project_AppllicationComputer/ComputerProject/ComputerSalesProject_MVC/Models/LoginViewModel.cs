namespace ComputerSalesProject_MVC.Models
{
    public sealed class LoginViewModel
    {
        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
    }
}
