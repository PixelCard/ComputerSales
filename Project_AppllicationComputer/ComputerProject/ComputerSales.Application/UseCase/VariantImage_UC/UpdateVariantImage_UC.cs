using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.Category_DTO;
using ComputerSales.Application.UseCaseDTO.VariantImage;
using ComputerSales.Application.UseCaseDTO.VariantImageDTO;
using ComputerSales.Domain.Entity.EVariant;

namespace ComputerSales.Application.UseCase.VariantImage_UC
{
    public class UpdateVariantImage_UC
    {
        private IRespository<VariantImage> respository;

        private IUnitOfWorkApplication unitOfWorkApplication;

        public UpdateVariantImage_UC(IRespository<VariantImage> respository,
            IUnitOfWorkApplication unitOfWorkApplication)
        {
            this.respository = respository;
            this.unitOfWorkApplication = unitOfWorkApplication;
        }

        public async Task<VariantImageOutputDTO?> HandleAsync(VariantImageOutputDTO input, CancellationToken ct)
        {
            VariantImage entity = await respository.GetByIdAsync(input.Id);

            if (entity == null)
            {
                return null;
            }
            entity.VariantId = input.VariantId;
            entity.Url = input.Url;
            entity.DescriptionImg = input.DescriptionImg;
            entity.SortOrder = input.SortOrder;
         

            respository.Update(entity);

            await unitOfWorkApplication.SaveChangesAsync(ct);

            return entity.ToResult();
        }
    }
}
