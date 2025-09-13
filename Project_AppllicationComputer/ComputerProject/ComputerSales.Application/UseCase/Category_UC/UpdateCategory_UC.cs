using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.Category_DTO;
using ComputerSales.Application.UseCaseDTO.Provider_DTO;
using ComputerSales.Domain.Entity.ECategory;

namespace ComputerSales.Application.UseCase.Category_UC
{
    public class UpdateCategory_UC
    {
        private IRespository<Accessories> respository;

        private IUnitOfWorkApplication unitOfWorkApplication;

        public UpdateCategory_UC(IRespository<Accessories> respository,
            IUnitOfWorkApplication unitOfWorkApplication)
        {
            this.respository = respository;
            this.unitOfWorkApplication = unitOfWorkApplication;
        }

        public async Task<CategoryOutput?> HandleAsync(CategoryOutput input, CancellationToken ct)
        {
            Accessories entity = await respository.GetByIdAsync(input.id);

            if (entity == null)
            {
                return null;
            }

            entity.Name= input.name;

            respository.Update(entity);

            await unitOfWorkApplication.SaveChangesAsync(ct);

            return entity.ToResult();
        }
    }
}
