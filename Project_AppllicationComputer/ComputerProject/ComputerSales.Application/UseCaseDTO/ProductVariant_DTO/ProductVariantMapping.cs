using ComputerSales.Domain.Entity.EProduct;

namespace ComputerSales.Application.UseCaseDTO.ProductVariant_DTO
{
    public static class ProductVariantMapping
    {
        public static ProductVariant ToEnity(this ProductVariantInput input)
        {
            return ProductVariant.create(
                input.ProductId,
                input.SKU,
                input.Status,
                input.Quantity,
                input.VariantName
            );
        }

        public static ProductVariantOutput ToResult(this ProductVariant e)
        {
            return new ProductVariantOutput(
                e.SKU, 
                e.Status, 
                e.Quantity,
                e.Id,
                e.VariantName
            );
        }
    }
}
