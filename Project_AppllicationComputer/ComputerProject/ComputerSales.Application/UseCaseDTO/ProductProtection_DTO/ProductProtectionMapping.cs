using ComputerSales.Domain.Entity.EProduct;

namespace ComputerSales.Application.UseCaseDTO.ProductProtection_DTO
{
    public static class ProductProtectionMapping
    {
        public static ProductProtection ToEnity(this ProductProtectionInputcs input)
        {
            return ProductProtection.create(
                input.ProductId,
                input.DateBuy,
                input.DateEnd,
                input.Status
            );
        }

        public static ProductProtectionOutput ToResult(this ProductProtection e)
        {
            return new ProductProtectionOutput(
               e.ProtectionProductId,
               e.DateBuy,
               e.DateEnd,
               e.Status,
               e.ProductId
            );
        }
    }
}
