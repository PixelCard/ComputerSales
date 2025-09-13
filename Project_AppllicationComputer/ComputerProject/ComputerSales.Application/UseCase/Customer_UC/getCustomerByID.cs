using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.Customer_DTO;
using ComputerSales.Application.UseCaseDTO.Customer_DTO.getCustomerByID;
using ComputerSales.Application.UseCaseDTO.ProductOverView_DTO;
using ComputerSales.Domain.Entity.ECustomer;

namespace ComputerSales.Application.UseCase.Customer_UC
{
    public class getCustomerByID
    {
        private IRespository<Customer> _repoCustomerGetID; //tạo biến repo bằng cách kế thừa phương thức IRespository
        private IUnitOfWorkApplication _unitOfWorkApplication;

        public getCustomerByID(IRespository<Customer> productOverView,
            IUnitOfWorkApplication unitOfWorkApplication)
        {
            _repoCustomerGetID = productOverView;
            _unitOfWorkApplication = unitOfWorkApplication;
        }

        public async Task<CustomerOutputDTO?> HandleAsync(InputGetCustomerByID input, CancellationToken ct)
        {
            var entity = await _repoCustomerGetID.GetByIdAsync(input.IDCustomer, ct);

            if (entity == null) return null;

            return entity.ToResult();
        }
    }
}
