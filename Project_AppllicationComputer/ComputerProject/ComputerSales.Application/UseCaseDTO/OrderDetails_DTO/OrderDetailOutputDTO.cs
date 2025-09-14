namespace ComputerSales.Application.UseCaseDTO.OrderDetail_DTO
{
    public sealed record OrderDetailOutputDTO(
        int OrderID,
        long? ProductID,
        int ProductVariantID,
        int Quantity,
        decimal UnitPrice,
        decimal Discount,
        decimal TotalPrice,
        string SKU,
        string Name,
        string OptionSummary,
        string ImageUrl
    );
}
