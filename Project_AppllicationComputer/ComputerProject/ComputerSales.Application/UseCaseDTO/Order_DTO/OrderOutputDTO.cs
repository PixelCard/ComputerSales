namespace ComputerSales.Application.UseCaseDTO.Order_DTO
{
    public sealed record OrderOutputDTO
    (
        int OrderID,
        DateTime OrderTime,
        int IDCustomer,
        int? PaymentID,
        int OrderStatus,
        decimal GrandTotal,
        bool Status
        );
}
