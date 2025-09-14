namespace ComputerSales.Application.UseCaseDTO.CartItem_DTO.UpdateCartItem
{
    public sealed record InputUpdateCartItem(
        int ID,
        int Quantity,
        bool IsSelected
    );
}
