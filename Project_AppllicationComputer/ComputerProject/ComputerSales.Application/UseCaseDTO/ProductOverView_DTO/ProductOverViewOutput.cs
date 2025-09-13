using ComputerSales.Domain.Entity.EProduct;

namespace ComputerSales.Application.UseCaseDTO.ProductOverView_DTO
{
    public sealed record ProductOverViewOutput(int ProductOverviewId, long ProductId, OverviewBlockType BlockType, string TextContent,
            string? ImageUrl, string? Caption, int DisplayOrder);
}
