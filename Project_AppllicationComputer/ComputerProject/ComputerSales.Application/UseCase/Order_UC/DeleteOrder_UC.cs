using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.Order_DTO;
using ComputerSales.Application.UseCaseDTO.Order_DTO.DeleteOrder;
using ComputerSales.Application.UseCaseDTO.ProductProtection_DTO;
using ComputerSales.Domain.Entity.E_Order;

namespace ComputerSales.Application.UseCase.Order_UC
{
    public class DeleteOrder_UC
    {
        private IRespository<Order> _repo;
        private IUnitOfWorkApplication _unitOfWorkApplication;

        public DeleteOrder_UC(IRespository<Order> order,
            IUnitOfWorkApplication unitOfWorkApplication)
        {
            _repo = order;
            _unitOfWorkApplication = unitOfWorkApplication;
        }

        public async Task<OrderOutputDTO?> HandleAsync(InputDeleteOrderByID input, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(input.OrderID, ct);
            if (entity == null) return null;

            _repo.Remove(entity);
            await _unitOfWorkApplication.SaveChangesAsync(ct);

            return entity.ToResult();
        }
    }
}
