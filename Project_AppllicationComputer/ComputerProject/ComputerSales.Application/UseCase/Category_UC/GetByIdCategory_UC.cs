using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.Category_DTO;
using ComputerSales.Application.UseCaseDTO.Provider_DTO;
using ComputerSales.Domain.Entity.ECategory;

namespace ComputerSales.Application.UseCase.Category_UC
{
    public class GetByIdCategory_UC
    {
        private IRespository<Accessories> respository;

        private IUnitOfWorkApplication unitOfWorkApplication;

        public GetByIdCategory_UC(IRespository<Accessories> respository,
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

            return entity.ToResult();
        }
    }
}
