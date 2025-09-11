using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.Provider_DTO;
using ComputerSales.Domain.Entity.EProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCase.Provider_UC
{
    public class DeleteProvider_UC
    {
        private IRespository<Provider> respository;

        private IUnitOfWorkApplication unitOfWorkApplication;

        public DeleteProvider_UC(IRespository<Provider> respository,
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

            respository.Remove(entity);

            await unitOfWorkApplication.SaveChangesAsync(ct);

            return entity.ToResult();
        }
    }
}
