using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.VariantPrice_DTO;
using ComputerSales.Application.UseCaseDTO.VariantPrice_DTO.VariantPriceInput_Output;
using ComputerSales.Domain.Entity.EVariant;

namespace ComputerSales.Application.UseCase.VariantPrice_UC
{
    public class GetByIdVariantPrice_UC
    {
        private readonly IRespository<VariantPrice> respository;
        private readonly IUnitOfWorkApplication unitOfWorkApplication;

        public GetByIdVariantPrice_UC(IRespository<VariantPrice> respository,
            IUnitOfWorkApplication unitOfWorkApplication)
        {
            this.respository = respository;
            this.unitOfWorkApplication = unitOfWorkApplication;
        }

        public async Task<VariantPriceOutputDTO?> HandleAsync(Guid id, CancellationToken ct)
        {
            VariantPrice entity = await respository.GetByIdAsync(id);

            if (entity == null)
            {
                return null;
            }

            return entity.ToResult();
        }
    }
}
