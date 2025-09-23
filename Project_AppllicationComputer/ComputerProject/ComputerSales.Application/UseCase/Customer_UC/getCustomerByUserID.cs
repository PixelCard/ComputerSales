using ComputerSales.Application.Interface.InterfaceCustomerRespo;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.Customer_DTO;
using ComputerSales.Application.UseCaseDTO.Customer_DTO.getCustomerByUserID;

namespace ComputerSales.Application.UseCase.Customer_UC
{
    public class getCustomerByUserID
    {
        private IUnitOfWorkApplication _unitOfWorkApplication;
        private ICustomerRespo customerRespo;

        public getCustomerByUserID(IUnitOfWorkApplication unitOfWorkApplication, ICustomerRespo customerRespo)
        {
            _unitOfWorkApplication = unitOfWorkApplication;
            this.customerRespo = customerRespo;
        }

        public async Task<CustomerOutputDTO?> HandleAsync(CustomerGetCustomerByUserID_Request input,CancellationToken ct)
        {
            var entity = await customerRespo.GetCustomerByUserID(input.userid, ct);

            if (entity == null) return null;

            return entity.ToResult();
        }
    }
}
