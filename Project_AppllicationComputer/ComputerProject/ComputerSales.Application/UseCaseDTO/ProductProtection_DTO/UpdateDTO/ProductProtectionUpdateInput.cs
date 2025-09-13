using ComputerSales.Domain.Entity.EProduct;

namespace ComputerSales.Application.UseCaseDTO.ProductProtection_DTO.UpdateDTO
{
    public sealed record ProductProtectionUpdateInput(
        long ProtectionProductId,
        DateTime DateBuy,
         DateTime DateEnd,
         WarrantyStatus Status);
}
