using ComputerSales.Application.UseCaseDTO.Order_DTO;

namespace ComputerSales.Application.UseCaseDTO.Order_DTO.GetOrdersList
{
    public sealed record GetOrdersListResultDTO
    {
        public List<OrderOutputDTO> Orders { get; init; } = new();
        public int TotalCount { get; init; }
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}

