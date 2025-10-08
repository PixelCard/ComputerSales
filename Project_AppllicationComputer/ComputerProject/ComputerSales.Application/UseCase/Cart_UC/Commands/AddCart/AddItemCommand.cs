namespace ComputerSales.Application.UseCase.Cart_UC.Commands.AddCart
{
    public sealed record AddItemCommand
    (
        int UserId, int ProductId, int? ProductVariantId, int Quantity, int? OptionalValueId = null
    );
}
