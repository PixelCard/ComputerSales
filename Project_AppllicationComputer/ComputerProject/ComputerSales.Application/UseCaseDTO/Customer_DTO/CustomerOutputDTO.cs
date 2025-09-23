namespace ComputerSales.Application.UseCaseDTO.Customer_DTO
{
    public sealed record CustomerOutputDTO(
     int IDCustomer,
     string? IMG,
     string Name,
     string? Description,
     string? sdt,
    string address,
     DateTime Date,
     int IDAccount
    );
}
