namespace ComputerSalesProject_MVC.Models
{
    public class CustomerSummaryViewModel
    {
        public int AccountId { get; init; }
        public string FullName { get; init; } = "";
        public string Email { get; init; } = "";
        public string Phone { get; init; } = "";
        public string Address { get; init; } = "";

        public static CustomerSummaryViewModel createCustomerSummaryViewModel(int accountid,string fullname,string email,string phone,string address)
        {
            return new CustomerSummaryViewModel
            {
                AccountId = accountid,
                FullName = fullname,
                Email = email,
                Phone = phone,
                Address = address
            };
        }
    }
}
