using ComputerSales.Domain.Entity.ECart;

namespace ComputerSales.Application.UseCaseDTO.Cart_DTO
{
    public static class CartMapping
    {
        public static Cart ToEntity(this CartInputDTO input)
        {
            return Cart.Create(
                input.UserID,
                input.Subtotal,
                input.DiscountTotal,
                input.ShippingFee,
                input.Status
            );
        }

        public static CartOutputDTO ToResult(this Cart entity)
        {
            return new CartOutputDTO(
                entity.ID,
                entity.UserID,
                entity.Status,
                entity.Subtotal,
                entity.DiscountTotal,
                entity.ShippingFee,
                entity.GrandTotal,
                entity.ExpiresAT,
                entity.CreatedAt,
                entity.UpdateAt
            );
        }
    }
}
