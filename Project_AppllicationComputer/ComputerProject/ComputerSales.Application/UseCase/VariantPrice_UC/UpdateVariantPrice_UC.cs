using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.VariantPrice_DTO;
using ComputerSales.Application.UseCaseDTO.VariantPrice_DTO.VariantPriceInput_Output;
using ComputerSales.Domain.Entity.EVariant;

namespace ComputerSales.Application.UseCase.VariantPrice_UC
{
    public class UpdateVariantPrice_UC
    {
        private readonly IRespository<VariantPrice> respository;
        private readonly IUnitOfWorkApplication unitOfWorkApplication;

        public UpdateVariantPrice_UC(IRespository<VariantPrice> respository,
            IUnitOfWorkApplication unitOfWorkApplication)
        {
            this.respository = respository;
            this.unitOfWorkApplication = unitOfWorkApplication;
        }

        public async Task<VariantPriceOutputDTO?> HandleAsync(VariantPriceInputDTO input, CancellationToken ct)
        {
            VariantPrice entity = await respository.GetByIdAsync(input.VariantId);

            if (entity == null)
            {
                return null;
            }

            // Gán lại các giá trị cần update
            entity.Price = input.Price;
            entity.DiscountPrice = input.DiscountPrice;
            entity.Currency = input.Currency;
            entity.Status = input.Status;
            entity.ValidFrom = input.ValidFrom;
            entity.ValidTo = input.ValidTo;
            entity.VariantId = input.VariantId;

            respository.Update(entity);

            await unitOfWorkApplication.SaveChangesAsync(ct);

            return entity.ToResult();
        }
    }
}
