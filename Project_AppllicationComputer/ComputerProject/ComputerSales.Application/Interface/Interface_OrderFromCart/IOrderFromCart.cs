using ComputerSales.Application.UseCaseDTO.Order_DTO;

namespace ComputerSales.Application.Interface.Interface_OrderFromCart
{
    public interface IOrderFromCart
    {
        Task<int> CreateFromCartAsync(int userId,string fullName, string phone, string? email, string address, string? notes,PaymentKind payment,CancellationToken ct);
    }
}
