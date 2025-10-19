using ComputerSales.Domain.Entity.EProduct;

namespace ComputerSales.Application.UseCaseDTO.ProductOverView_DTO
{
    public sealed record ProductOverViewInput(
        long ProductId,
        string TextContent
        );
}
