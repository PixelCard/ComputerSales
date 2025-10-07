using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.Provider_DTO;
using ComputerSales.Application.UseCaseDTO.VariantPrice_DTO;
using ComputerSales.Application.UseCaseDTO.VariantPrice_DTO.VariantPriceInput_Output;
using ComputerSales.Domain.Entity.EVariant;

namespace ComputerSales.Application.UseCase.VariantPrice_UC
{
    public class CreateVariantPrice_UC
    {
        private IRespository<VariantPrice> respository;

        private IUnitOfWorkApplication unitOfWorkApplication;

        public CreateVariantPrice_UC(IRespository<VariantPrice> respository,
            IUnitOfWorkApplication unitOfWorkApplication)
        {
            this.respository = respository;
            this.unitOfWorkApplication = unitOfWorkApplication;
        }

        public async Task<VariantPriceOutputDTO?> HandleAsync(VariantPriceInputDTO input, CancellationToken ct)
        {
            VariantPrice entity = input.ToEnity();

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