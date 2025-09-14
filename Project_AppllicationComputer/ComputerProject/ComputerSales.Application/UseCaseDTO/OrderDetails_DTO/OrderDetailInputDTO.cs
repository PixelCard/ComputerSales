namespace ComputerSales.Application.UseCaseDTO.OrderDetail_DTO
{
    public sealed record OrderDetailInputDTO(
        long? ProductID,
        int ProductVariantID,
        int Quantity,
        decimal UnitPrice,
        decimal Discount,
        string SKU,
        string Name,
        string OptionSummary,
        string ImageUrl
    );
}
