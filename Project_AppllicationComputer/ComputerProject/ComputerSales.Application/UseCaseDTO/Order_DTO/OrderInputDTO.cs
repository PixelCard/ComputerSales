namespace ComputerSales.Application.UseCaseDTO.Order_DTO
{
    public sealed record OrderInputDTO(
        DateTime OrderTime,
        int IDCustomer,
        int? PaymentID,
        int OrderStatus,
        decimal Subtotal,
        decimal DiscountTotal,
        decimal ShippingFee,
        decimal GrandTotal, // sẽ KHÔNG dùng, vì entity tự tính
        bool Status
    );
}
