using ComputerSales.Domain.Entity.ECart;

namespace ComputerSales.Application.UseCaseDTO.CartItem_DTO
{
    public static class CartItemMapping
    {
        public static CartItem ToEntity(this CartItemInputDTO input)
        {
            return new CartItem
            {
                CartID = input.CartID,
                ProductID = input.ProductID,
                ProductVariantID = input.ProductVariantID,
                ParentItemID = input.ParentItemID,
                ItemType = input.ItemType,
                SKU = input.SKU,
                Name = input.Name,
                OptionSummary = input.OptionSummary,
                ImageUrl = input.ImageUrl,
                UnitPrice = input.UnitPrice,
                Quantity = input.Quantity,
                PerItemLimit = input.PerItemLimit,
                CreatedAt = DateTime.UtcNow,
                IsSelected = input.IsSelected
            };
        }

        public static CartItemOutputDTO ToResult(this CartItem e)
        {
            return new CartItemOutputDTO(
                e.ID,
                e.CartID,
                e.ProductID,
                e.ProductVariantID,
                e.ParentItemID,
                e.ItemType,
                e.SKU,
                e.Name,
                e.OptionSummary,
                e.ImageUrl,
                e.UnitPrice,
                e.Quantity,
                e.PerItemLimit,
                e.CreatedAt,
                e.IsSelected
            );
        }
    }
}
