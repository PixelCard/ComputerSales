using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.Provider_DTO;
using ComputerSales.Domain.Entity.EProvider;

namespace ComputerSales.Application.UseCase.Provider_UC
{
    public class GetByIdProvider_UC
    {
        private IRespository<Provider> respository;

        private IUnitOfWorkApplication unitOfWorkApplication;

        public GetByIdProvider_UC(IRespository<Provider> respository,
            IUnitOfWorkApplication unitOfWorkApplication)
        {
            this.respository = respository;
            this.unitOfWorkApplication = unitOfWorkApplication;
        }

        public async Task<ProviderOutput?> HandleAsync(ProviderOutput input, CancellationToken ct)
        {
            Provider entity = await respository.GetByIdAsync(input.ProviderID);

            if (entity == null)
            {
                return null;
            }

            return entity.ToResult();
        }
    }
}
