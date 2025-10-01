using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.Category_DTO;
using ComputerSales.Application.UseCaseDTO.VariantImage;
using ComputerSales.Application.UseCaseDTO.VariantImageDTO;
using ComputerSales.Domain.Entity.EVariant;

namespace ComputerSales.Application.UseCase.VariantImage_UC
{
    public class getVariantImageById_UC
    {
        private IRespository<VariantImage> respository;

        private IUnitOfWorkApplication unitOfWorkApplication;

        public getVariantImageById_UC(IRespository<VariantImage> respository,
            IUnitOfWorkApplication unitOfWorkApplication)
        {
            this.respository = respository;
            this.unitOfWorkApplication = unitOfWorkApplication;
        }

        public async Task<VariantImageOutputDTO?> HandleAsync(int id, CancellationToken ct)
        {
            VariantImage entity = await respository.GetByIdAsync(id, ct);

            if (entity == null)
            {
                return null;
            }

            return entity.ToResult(); // trả đúng OutputDTO
        }

    }
}
