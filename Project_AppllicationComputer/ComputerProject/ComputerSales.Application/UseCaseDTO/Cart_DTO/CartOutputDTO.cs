namespace ComputerSales.Application.UseCaseDTO.Cart_DTO
{
    public sealed record CartOutputDTO(
        int ID,
        int UserID,
        int Status,
        decimal Subtotal,
        decimal DiscountTotal,
        decimal ShippingFee,
        decimal GrandTotal,
        DateTime? ExpiresAT,
        DateTime CreatedAt,
        DateTime UpdateAt
    );
}
