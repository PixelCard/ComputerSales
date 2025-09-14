namespace ComputerSales.Application.UseCaseDTO.Cart_DTO
{
    public sealed record CartInputDTO(
        int UserID,
        int Status,
        decimal Subtotal,
        decimal DiscountTotal,
        decimal ShippingFee,
        DateTime? ExpiresAT
    );
}
