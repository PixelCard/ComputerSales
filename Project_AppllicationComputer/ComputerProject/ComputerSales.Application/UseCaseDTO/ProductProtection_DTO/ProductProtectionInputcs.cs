using ComputerSales.Domain.Entity.EProduct;

namespace ComputerSales.Application.UseCaseDTO.ProductProtection_DTO
{
    public sealed record ProductProtectionInputcs(
        DateTime DateBuy,
        DateTime DateEnd,
        WarrantyStatus Status,
        long ProductId);
}
