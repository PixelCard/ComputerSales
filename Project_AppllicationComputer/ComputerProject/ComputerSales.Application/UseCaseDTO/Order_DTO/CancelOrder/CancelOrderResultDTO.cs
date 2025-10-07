using ComputerSales.Application.UseCaseDTO.Order_DTO;

namespace ComputerSales.Application.UseCaseDTO.Order_DTO.CancelOrder
{
    public sealed record CancelOrderResultDTO
    {
        public bool Success { get; init; }
        public string Message { get; init; } = string.Empty;
        public OrderOutputDTO? Order { get; init; }
    }
}
