using ComputerSales.Domain.Entity.EProduct;

namespace ComputerSales.Application.UseCaseDTO.ProductVariant_DTO
{
    public sealed record ProductVariantInput(
     long ProductId,
     string SKU ,
     VariantStatus Status ,
     int Quantity,
     string VariantName);
}
