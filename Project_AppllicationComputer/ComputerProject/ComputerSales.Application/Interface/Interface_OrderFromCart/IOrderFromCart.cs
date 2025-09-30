using ComputerSales.Application.UseCaseDTO.Order_DTO;

namespace ComputerSales.Application.Interface.Interface_OrderFromCart
{
    public interface IOrderFromCart
    {
        //HÀM NÀY để “chốt” đơn đã thanh toán(gắn phương thức thanh toán, đổi trạng thái, lưu log)
        Task MarkPaidAsync(int orderId, PaymentKind payment, string transactionId, string? responseCode, CancellationToken ct);
        Task<int> CreateFromCartAsync(int userId,string fullName, string phone, string? email, string address, string? notes,PaymentKind payment,CancellationToken ct);
    }
}
