using ComputerSales.Application.UseCaseDTO.Account_DTO;
using ComputerSales.Application.UseCaseDTO.Customer_DTO;

namespace ComputerSalesProject_MVC.Models.Acc_Cus_ViewModel
{
    public class AccountDetailsVM
    {
        public AccountOutputDTO Account { get; set; } = default!;
        public CustomerOutputDTO? Customer { get; set; }   // có thể chưa có
    }
}
