namespace ComputerSales.Application.UseCaseDTO.Order_DTO.CancelOrder
{
    public sealed record InputCancelOrder(
        int OrderID,
        string? CancelReason = null
    );
}
