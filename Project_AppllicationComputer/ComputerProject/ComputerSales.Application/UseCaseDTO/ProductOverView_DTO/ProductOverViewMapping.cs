using ComputerSales.Domain.Entity.EProduct;

namespace ComputerSales.Application.UseCaseDTO.ProductOverView_DTO
{
    public static class ProductOverViewMapping
    {
        public static ProductOverview ToEntity(this ProductOverViewInput input)
        {
            return ProductOverview.Create(
                input.ProductId,
                input.BlockType,
                input.TextContent,
                string.IsNullOrWhiteSpace(input.ImageUrl) ? null : input.ImageUrl.Trim(),
                string.IsNullOrWhiteSpace(input.Caption) ? null : input.Caption.Trim(),
                input.DisplayOrder
            );
        }

        public static ProductOverViewOutput ToResult(this ProductOverview e)
        {
            return new ProductOverViewOutput(
                e.ProductOverviewId,
                e.ProductId,
                e.BlockType,
                e.TextContent,
                e.ImageUrl,
                e.Caption,
                e.DisplayOrder,
                e.CreateDate
            );
        }
    }
}
