using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.Customer_DTO;
using ComputerSales.Application.UseCaseDTO.ProductOverView_DTO;
using ComputerSales.Domain.Entity.ECustomer;

namespace ComputerSales.Application.UseCase.Customer_UC
{
    public class DeleteCustomer_UC
    {
        private IRespository<Customer> _repoDeleteCustomer;
        private IUnitOfWorkApplication _unitOfWorkApplication;

        public DeleteCustomer_UC(IRespository<Customer> productOverView,
            IUnitOfWorkApplication unitOfWorkApplication)
        {
            _repoDeleteCustomer = productOverView;
            _unitOfWorkApplication = unitOfWorkApplication;
        }

        public async Task<CustomerOutputDTO> HandleAsync(CustomerOutputDTO input, CancellationToken ct)
        {
            var entity = await _repoDeleteCustomer.GetByIdAsync(input.IDCustomer, ct);
            if (entity == null) return null;

            _repoDeleteCustomer.Remove(entity);
            await _unitOfWorkApplication.SaveChangesAsync(ct);

            return entity.ToResult();
        }
    }
}
