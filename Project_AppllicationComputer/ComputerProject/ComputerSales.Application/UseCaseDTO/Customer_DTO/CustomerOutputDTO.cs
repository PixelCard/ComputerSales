namespace ComputerSales.Application.UseCaseDTO.Customer_DTO
{
    public sealed record CustomerOutputDTO(
     int IDCustomer,
     string? IMG,
     string Name,
     string? Description,
     DateTime Date,
     int IDAccount
    );
}
