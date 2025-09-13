using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.Account_DTO;
using ComputerSales.Application.UseCaseDTO.Order_DTO;
using ComputerSales.Application.UseCaseDTO.Order_DTO.GetOrderByID;
using ComputerSales.Application.UseCaseDTO.ProductProtection_DTO;
using ComputerSales.Domain.Entity.E_Order;

namespace ComputerSales.Application.UseCase.Order_UC
{
    public class GetOrderByIDCustomerAndOrderID
    {
        private IRespository<Order> _repo;
        private IUnitOfWorkApplication _unitOfWorkApplication;

        public GetOrderByIDCustomerAndOrderID(IRespository<Order> repo,
            IUnitOfWorkApplication unitOfWorkApplication)
        {
            _repo = repo;
            _unitOfWorkApplication = unitOfWorkApplication;
        }

        public async Task<OrderOutputDTO?> HandleAsync(InputGetOrderByandCustomerID input, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(input.OrderID, ct);

            if (entity == null) return null;

            return entity.ToResult();
        }
    }
}
