using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.Category_DTO;
using ComputerSales.Application.UseCaseDTO.NewFolder;
using ComputerSales.Application.UseCaseDTO.Role_DTO;
using ComputerSales.Application.UseCaseDTO.VariantImage;
using ComputerSales.Application.UseCaseDTO.VariantImageDTO;
using ComputerSales.Domain.Entity.EVariant;

namespace ComputerSales.Application.UseCase.VariantImage_UC
{
    public class CreateVariantImage_UC
    {
        private IRespository<VariantImage> respository;

        private IUnitOfWorkApplication unitOfWorkApplication;

        public CreateVariantImage_UC(IRespository<VariantImage> respository,
            IUnitOfWorkApplication unitOfWorkApplication)
        {
            this.respository = respository;
            this.unitOfWorkApplication = unitOfWorkApplication;
        }

        public async Task<VariantImageOutputDTO?> HandleAsync(VariantImageInputDTO input, CancellationToken ct)
        {
            VariantImage entity = input.ToEntity();

            if (entity == null)
            {
                return null;
            }

            await respository.AddAsync(entity, ct);

            await unitOfWorkApplication.SaveChangesAsync(ct);

            return entity.ToResult();
        }
    }
}
