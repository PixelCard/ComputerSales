using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.VariantImage;
using ComputerSales.Application.UseCaseDTO.VariantImage.DeleteVariantImage;
using ComputerSales.Domain.Entity.EVariant;

namespace ComputerSales.Application.UseCase.VariantImage_UC
{
    public class DeleteVariantImage_UC
    {
        private readonly IRespository<VariantImage> respository;
        private readonly IUnitOfWorkApplication unitOfWorkApplication;

        public DeleteVariantImage_UC(
            IRespository<VariantImage> respository,
            IUnitOfWorkApplication unitOfWorkApplication)
        {
            this.respository = respository;
            this.unitOfWorkApplication = unitOfWorkApplication;
        }

        public async Task<bool> HandleAsync(DeleteVariantImageInput input, CancellationToken ct)
        {
            var entity = await respository.GetByIdAsync(input.ID, ct);
            if (entity == null)
            {
                return false;
            }

            respository.Remove(entity);
            await unitOfWorkApplication.SaveChangesAsync(ct);

            return true;
        }
    }
}
