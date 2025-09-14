namespace ComputerSales.Application.UseCaseDTO.CartItem_DTO
{
    public sealed record CartItemOutputDTO(
        int ID,
        int CartID,
        int? ProductID,
        int? ProductVariantID,
        int? ParentItemID,
        int ItemType,
        string SKU,
        string Name,
        string OptionSummary,
        string ImageUrl,
        decimal UnitPrice,
        int Quantity,
        int? PerItemLimit,
        DateTime CreatedAt,
        bool IsSelected
    );
}
