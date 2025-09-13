using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.Order_DTO;          // OrderInputDTO, OrderOutputDTO, ToResult()
using ComputerSales.Domain.Entity.E_Order;
using System.Reflection;

namespace ComputerSales.Application.UseCase.Order_UC
{
    public class UpdateOrder_UC
    {
        private readonly IRespository<Order> _repoOrder;
        private readonly IUnitOfWorkApplication _unitOfWork;

        public UpdateOrder_UC(
            IRespository<Order> repoOrder,
            IUnitOfWorkApplication unitOfWork)
        {
            _repoOrder = repoOrder;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Cập nhật Order với dữ liệu đầy đủ từ OrderInputDTO.
        /// orderId lấy từ route/query; không cho đổi chủ đơn (IDCustomer).
        /// GrandTotal sẽ được tính lại, KHÔNG dùng giá trị trong DTO.
        /// </summary>
        public async Task<OrderOutputDTO?> HandleAsync(int orderId, OrderInputDTO input, CancellationToken ct = default)
        {
            var entity = await _repoOrder.GetByIdAsync(orderId, ct);
            if (entity is null) return null;

            // Không cho phép đổi chủ đơn.
            if (entity.IDCustomer != input.IDCustomer)
            {
                // Có thể: return null; hoặc throw InvalidOperationException;
                // Ở đây mình chọn: giữ nguyên IDCustomer hiện tại và bỏ qua input.
                // Nếu bạn muốn chặt chẽ hơn, hãy throw:
                // throw new InvalidOperationException("Không được đổi chủ sở hữu đơn hàng.");
            }

            // Cập nhật các trường cho phép
            entity.OrderTime = input.OrderTime;
            entity.PaymentID = input.PaymentID;
            entity.OrderStatus = input.OrderStatus;
            entity.Subtotal = input.Subtotal;
            entity.DiscountTotal = input.DiscountTotal;
            entity.ShippingFee = input.ShippingFee;
            entity.Status = input.Status;

            // TÍNH LẠI GrandTotal (bỏ qua input.GrandTotal)
            SetGrandTotal(entity, entity.Subtotal - entity.DiscountTotal + entity.ShippingFee);

            _repoOrder.Update(entity);
            await _unitOfWork.SaveChangesAsync(ct);

            return entity.ToResult();
        }

        private static void SetGrandTotal(Order order, decimal value)
        {
            // do GrandTotal có private set => dùng reflection để set
            var prop = typeof(Order).GetProperty(nameof(Order.GrandTotal),
                BindingFlags.Instance | BindingFlags.Public);
            var setMethod = prop?.GetSetMethod(true);
            setMethod?.Invoke(order, new object[] { value });
        }
    }
}
