using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.Order_DTO;
using ComputerSales.Application.UseCaseDTO.Order_DTO.CancelOrder;
using ComputerSales.Domain.Entity.E_Order;
using System.Reflection;

namespace ComputerSales.Application.UseCase.Order_UC
{
    public class CancelOrder_UC
    {
        private readonly IRespository<Order> _repoOrder;
        private readonly IUnitOfWorkApplication _unitOfWork;

        public CancelOrder_UC(
            IRespository<Order> repoOrder,
            IUnitOfWorkApplication unitOfWork)
        {
            _repoOrder = repoOrder;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Hủy đơn hàng với validation trạng thái.
        /// Chỉ cho phép hủy khi đơn hàng chưa giao (DangGiao, DaGiaoThanhCong).
        /// </summary>
        public async Task<CancelOrderResultDTO> HandleAsync(InputCancelOrder input, CancellationToken ct = default)
        {
            var entity = await _repoOrder.GetByIdAsync(input.OrderID, ct);
            if (entity is null)
            {
                return new CancelOrderResultDTO
                {
                    Success = false,
                    Message = "Không tìm thấy đơn hàng",
                    Order = null
                };
            }

            // Kiểm tra trạng thái đơn hàng
            if (entity.OrderStatus == OrderStatus.DangGiao || entity.OrderStatus == OrderStatus.DaGiaoThanhCong)
            {
                return new CancelOrderResultDTO
                {
                    Success = false,
                    Message = "Không thể hủy đơn hàng đang giao hoặc đã giao thành công",
                    Order = entity.ToResult()
                };
            }

            // Kiểm tra nếu đã hủy rồi
            if (entity.OrderStatus == OrderStatus.DaHuy)
            {
                return new CancelOrderResultDTO
                {
                    Success = false,
                    Message = "Đơn hàng đã được hủy trước đó",
                    Order = entity.ToResult()
                };
            }

            // Cập nhật trạng thái thành đã hủy
            entity.OrderStatus = OrderStatus.DaHuy;
            entity.OrderNote = string.IsNullOrEmpty(input.CancelReason) 
                ? entity.OrderNote 
                : $"{entity.OrderNote}\n[Đã hủy: {input.CancelReason}]";

            _repoOrder.Update(entity);
            await _unitOfWork.SaveChangesAsync(ct);

            return new CancelOrderResultDTO
            {
                Success = true,
                Message = "Hủy đơn hàng thành công",
                Order = entity.ToResult()
            };
        }
    }
}
