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

        public async Task<VariantPriceOutputDTO?> HandleAsync(int id, VariantPriceInputDTO input, CancellationToken ct)
        {
            // tìm VariantPrice theo Id (PK)
            VariantPrice entity = await respository.GetByIdAsync(id);
            if (entity == null) return null;

            // update field
            entity.Price = input.Price;
            entity.DiscountPrice = input.DiscountPrice;
            entity.Currency = input.Currency;
            entity.Status = input.Status;
            entity.ValidFrom = input.ValidFrom;
            entity.ValidTo = input.ValidTo;
            entity.VariantId = input.VariantId; // nếu bạn cho phép đổi Variant

            respository.Update(entity);
            await unitOfWorkApplication.SaveChangesAsync(ct);

            return entity.ToResult();
        }

    }
}
