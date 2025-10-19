using ComputerSales.Domain.Entity.EProduct;

namespace ComputerSales.Application.UseCaseDTO.ProductOverView_DTO
{
    public static class ProductOverViewMapping
    {
        public static ProductOverview ToEntity(this ProductOverViewInput input)
        {
            return ProductOverview.Create(
                input.ProductId,
                input.TextContent
            );
        }

        public static ProductOverViewOutput ToResult(this ProductOverview e)
        {
            return new ProductOverViewOutput(
                e.ProductOverviewId,
                e.ProductId,
                e.TextContent,
                e.CreateDate
            );
        }
    }
}
