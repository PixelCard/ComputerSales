using ComputerSales.Application.Interface.InterFace_ProductOptionalType_Respository;
using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.ProductOptionalType_DTO;
using ComputerSales.Domain.Entity.EProduct;

namespace ComputerSales.Application.UseCase.ProductOptionalType_UC
{
    public class UpdateProductOptionalType_UC
    {
        private IUnitOfWorkApplication unitOfWorkApplication;
        private IRespository<ProductOptionType> _Respository;
        private IProductOptionalTypeRespositorycs productOptionalTypeRespositorycs;

        public UpdateProductOptionalType_UC(IUnitOfWorkApplication unitOfWorkApplication, 
            IRespository<ProductOptionType> respository,
            IProductOptionalTypeRespositorycs _productOptionalTypeRespositorycs)
        {
            this.unitOfWorkApplication = unitOfWorkApplication;
            _Respository = respository;
            productOptionalTypeRespositorycs= _productOptionalTypeRespositorycs;
        }
        
        public async Task<ProductOptionalTypeOutput?> HandleAsync(ProducyOptionalTypeInput input,
            CancellationToken ct)
        {
            var entity = await productOptionalTypeRespositorycs.GetByTwoIdAsync(input.ProductId,input.OptionTypeId, ct);
            if (entity == null) return null;

            entity.ProductId = input.ProductId;

            entity.OptionTypeId = input.OptionTypeId;

            await unitOfWorkApplication.SaveChangesAsync(ct);

            return entity.ToResult();
        }

    }
}
