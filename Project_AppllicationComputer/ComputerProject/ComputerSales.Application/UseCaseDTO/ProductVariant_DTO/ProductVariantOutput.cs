using ComputerSales.Domain.Entity.EProduct;

namespace ComputerSales.Application.UseCaseDTO.ProductVariant_DTO
{
    public sealed record ProductVariantOutput(
     string SKU,
     VariantStatus Status,
     long ProductId,
     int Quantity,
     int Id,
     string VariantName);
}
