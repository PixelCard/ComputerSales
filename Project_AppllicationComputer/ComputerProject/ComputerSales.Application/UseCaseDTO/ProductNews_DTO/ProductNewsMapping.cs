using ComputerSales.Application.UseCaseDTO.ProductProtection_DTO;
using ComputerSales.Domain.Entity.ENews;
using ComputerSales.Domain.Entity.EProduct;


namespace ComputerSales.Application.UseCaseDTO.ProductNews_DTO
{
    public static class ProductNewsMapping
    {
        public static ProductNews ToEnity(this ProductNewsInputDTO input)
        {
            return ProductNews.Create(
                input.BlockType,
                input.TextContent,
                input.ImageUrl,
                input.Caption,
                input.DisplayOrder,
                input.CreateDate
            );
        }

        public static ProductNewsOutputDTO ToResult(this ProductNews e)
        {
            return new ProductNewsOutputDTO(
              e.ProductNewsID,
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
