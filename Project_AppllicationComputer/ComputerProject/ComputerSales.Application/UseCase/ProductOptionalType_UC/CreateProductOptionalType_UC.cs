using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.ProductOptionalType_DTO;
using ComputerSales.Domain.Entity.EProduct;

namespace ComputerSales.Application.UseCase.ProductOptionalType_UC
{
    public class CreateProductOptionalType_UC
    {
        private IUnitOfWorkApplication unitOfWorkApplication;
        private IRespository<ProductOptionType> _Respository;

        public CreateProductOptionalType_UC(IUnitOfWorkApplication unitOfWorkApplication, IRespository<ProductOptionType> respository)
        {
            this.unitOfWorkApplication = unitOfWorkApplication;
            _Respository = respository;
        }

        public async Task<ProductOptionalTypeOutput?> HandleAsync(ProducyOptionalTypeInput 
            input, CancellationToken ct)
        {
            ProductOptionType entity = input.ToEnity();

            await _Respository.AddAsync(entity,ct);

            await unitOfWorkApplication.SaveChangesAsync();

            return entity.ToResult();

        }
    }
}
