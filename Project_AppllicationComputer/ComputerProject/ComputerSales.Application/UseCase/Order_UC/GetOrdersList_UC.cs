using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.Order_DTO;
using ComputerSales.Application.UseCaseDTO.Order_DTO.GetOrdersList;
using ComputerSales.Domain.Entity.E_Order;

namespace ComputerSales.Application.UseCase.Order_UC
{
    public class GetOrdersList_UC
    {
        private readonly IRespository<Order> _repoOrder;
        private readonly IUnitOfWorkApplication _unitOfWork;

        public GetOrdersList_UC(
            IRespository<Order> repoOrder,
            IUnitOfWorkApplication unitOfWork)
        {
            _repoOrder = repoOrder;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Lấy danh sách đơn hàng với filter theo trạng thái và phân trang
        /// </summary>
        public async Task<GetOrdersListResultDTO> HandleAsync(InputGetOrdersList input, CancellationToken ct = default)
        {
            // Tạo predicate để filter where theo trạng thái nếu có
            System.Linq.Expressions.Expression<Func<Order, bool>>? predicate = null;

            if (input.StatusFilter.HasValue)
            {
                predicate = order => order.OrderStatus == input.StatusFilter.Value;
            }

            // Order by OrderTime descending (mới nhất trước)
            Func<IQueryable<Order>, IOrderedQueryable<Order>> orderBy = query => query.OrderByDescending(o => o.OrderTime);

            // Lấy danh sách đơn hàng
            var orders = await _repoOrder.ListAsync(
                predicate: predicate,
                orderBy: orderBy,
                includes: null,
                skip: input.Skip,
                take: input.Take,
                ct: ct
            );

            // Đếm tổng số đơn hàng 
            var totalCount = await _repoOrder.ListAsync(
                predicate: predicate,
                orderBy: null,
                includes: null,
                skip: null,
                take: null,
                ct: ct
            );

            // Map sang DTO
            var orderDTOs = orders.Select(o => o.ToResult()).ToList();

            return new GetOrdersListResultDTO
            {
                Orders = orderDTOs,
                TotalCount = totalCount.Count,
                PageNumber = input.PageNumber,
                PageSize = input.PageSize
            };
        }
    }
}
