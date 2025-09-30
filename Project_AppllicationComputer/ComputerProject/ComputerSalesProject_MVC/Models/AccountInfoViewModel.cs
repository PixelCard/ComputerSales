namespace ComputerSalesProject_MVC.Models
{
    public class AccountInfoViewModel
    {
        public int AccountId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string MaskedPassword { get; set; } = "********";
    }
}


