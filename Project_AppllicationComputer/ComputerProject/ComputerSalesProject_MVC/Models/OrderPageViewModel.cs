using ComputerSales.Application.UseCaseDTO.Cart_DTO.Cart_Page;
using ComputerSales.Application.UseCaseDTO.Order_DTO;

namespace ComputerSalesProject_MVC.Models
{
    public sealed class OrderPageViewModel
    {
        public required CustomerSummaryViewModel CustomerSummary { get; init; }

        public required CartPageDTO cartPageDTO { get; init; }

        public required OrderCheckoutInput orderCheckoutInput { get; init; }
    }
}
