using ComputerSales.Domain.Entity.EProduct;

namespace ComputerSales.Application.UseCaseDTO.ProductOverView_DTO
{
    public sealed record ProductOverViewOutput(
        int ProductOverviewId,
        long ProductId,
        string TextContent,
        DateTime CreateDate
        );
}