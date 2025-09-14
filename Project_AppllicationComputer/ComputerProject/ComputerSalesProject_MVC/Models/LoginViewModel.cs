namespace ComputerSalesProject_MVC.Models
{
    public class LoginViewModel
    {
        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
    }
}
