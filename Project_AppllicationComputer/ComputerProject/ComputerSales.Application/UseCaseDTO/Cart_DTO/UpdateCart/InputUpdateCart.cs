

namespace ComputerSales.Application.UseCaseDTO.Cart_DTO.UpdateCart
{
    public sealed record InputUpdateCart(
      int IDCart,
    int UserID,
    int Status,
    decimal Subtotal,
    decimal DiscountTotal,
    decimal ShippingFee,
    DateTime? ExpiresAT
      );
}
