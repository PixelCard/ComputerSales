using ComputerSales.Domain.Entity.EProduct;

namespace ComputerSales.Application.UseCaseDTO.ProductVariant_DTO.UpdateDTO
{
    public sealed record UpdateDTO_ProductVariant(int Id,
         long ProductId,
         string SKU,
         VariantStatus Status,
         int Quantity);
}
