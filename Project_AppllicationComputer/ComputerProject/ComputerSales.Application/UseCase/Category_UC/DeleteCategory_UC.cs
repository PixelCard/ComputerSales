using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.Category_DTO;
using ComputerSales.Application.UseCaseDTO.Provider_DTO;
using ComputerSales.Domain.Entity.ECategory;
using ComputerSales.Domain.Entity.EProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCase.Category_UC
{
    public class DeleteCategory_UC
    {
        private IRespository<Accessories> respository;

        private IUnitOfWorkApplication unitOfWorkApplication;

        public DeleteCategory_UC(IRespository<Accessories> respository,
            IUnitOfWorkApplication unitOfWorkApplication)
        {
            this.respository = respository;
            this.unitOfWorkApplication = unitOfWorkApplication;
        }

        public async Task<CategoryOutput?> HandleAsync(CategoryOutput input, CancellationToken ct)
        {
            Accessories entity = await respository.GetByIdAsync(input.id,ct);

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
