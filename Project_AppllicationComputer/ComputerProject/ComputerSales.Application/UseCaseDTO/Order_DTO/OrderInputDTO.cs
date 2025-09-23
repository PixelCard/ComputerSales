using ComputerSales.Domain.Entity.E_Order;

namespace ComputerSales.Application.UseCaseDTO.Order_DTO
{
    public sealed record OrderInputDTO(
        DateTime OrderTime,
        int IDCustomer,
        int? PaymentID,
        OrderStatus OrderStatus,
        string? OrderNote,
        decimal Subtotal,
        decimal DiscountTotal,
        decimal ShippingFee,
        decimal GrandTotal, // sẽ KHÔNG dùng, vì entity tự tính
        bool Status
    );
}
