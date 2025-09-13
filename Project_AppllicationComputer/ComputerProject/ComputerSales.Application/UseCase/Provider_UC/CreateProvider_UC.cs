using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.Provider_DTO;
using ComputerSales.Domain.Entity.EProvider;

namespace ComputerSales.Application.UseCase.Provider_UC
{
    public class CreateCategory_UC
    {
        private IRespository<Provider> respository;

        private IUnitOfWorkApplication unitOfWorkApplication;

        public CreateCategory_UC(IRespository<Provider> respository, 
            IUnitOfWorkApplication unitOfWorkApplication)
        {
            this.respository = respository;
            this.unitOfWorkApplication = unitOfWorkApplication;
        }

        public async Task<ProviderOutput?> HandleAsync(ProviderInput input,CancellationToken ct)
        {
            Provider entity = input.ToEnity();

            if(entity == null )
            {
                return null;
            }

            await respository.AddAsync(entity, ct);

            await unitOfWorkApplication.SaveChangesAsync(ct);

            return entity.ToResult();
        }
    }
}
