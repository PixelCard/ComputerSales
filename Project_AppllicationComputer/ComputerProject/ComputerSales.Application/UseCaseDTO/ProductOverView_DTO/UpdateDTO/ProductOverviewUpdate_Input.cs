using ComputerSales.Domain.Entity.EProduct;

namespace ComputerSales.Application.UseCaseDTO.ProductOverView_DTO.UpdateDTO
{
    public sealed record ProductOverviewUpdate_Input(
        int ProductOverviewId,
        string? TextContent,
        string? ImageUrl,
        string? Caption,
        int? DisplayOrder,
        OverviewBlockType? BlockType
    );
}
