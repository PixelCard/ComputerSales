using ComputerSales.Domain.Entity.E_Order;

namespace ComputerSales.Application.UseCaseDTO.Order_DTO.GetOrdersList
{
    public sealed record InputGetOrdersList(
        OrderStatus? StatusFilter = null,
        int PageNumber = 1,
        int PageSize = 10
    )
    {
        public int Skip => (PageNumber - 1) * PageSize;
        public int Take => PageSize;
    }
}

