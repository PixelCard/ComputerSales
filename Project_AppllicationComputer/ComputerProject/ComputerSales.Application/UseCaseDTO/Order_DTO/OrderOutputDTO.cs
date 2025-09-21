using ComputerSales.Domain.Entity.E_Order;

namespace ComputerSales.Application.UseCaseDTO.Order_DTO
{
    public sealed record OrderOutputDTO
    (
        int OrderID,
        DateTime OrderTime,
        int IDCustomer,
        string? OrderNote,
        int? PaymentID,
        OrderStatus OrderStatus,
        decimal GrandTotal,
        bool Status
        );
}
