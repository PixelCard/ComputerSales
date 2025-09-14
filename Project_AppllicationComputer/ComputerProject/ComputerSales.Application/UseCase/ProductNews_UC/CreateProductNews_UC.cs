using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.ProductNews_DTO;
using ComputerSales.Application.UseCaseDTO.ProductOptionalType_DTO;
using ComputerSales.Domain.Entity.ENews;
 


namespace ComputerSales.Application.UseCase.ProductNews_UC
{
    public class CreateProductNews_UC
    {

        private IUnitOfWorkApplication unitOfWorkApplication;
        private IRespository<ProductNews> _Respository;

        public CreateProductNews_UC(IUnitOfWorkApplication unitOfWorkApplication, IRespository<ProductNews> respository)
        {
            this.unitOfWorkApplication = unitOfWorkApplication;
            _Respository = respository;
        }

        public async Task<ProductNewsOutputDTO?> HandleAsync(ProductNewsInputDTO
            input, CancellationToken ct)
        {
            ProductNews entity = input.ToEnity();

            await _Respository.AddAsync(entity, ct);

            await unitOfWorkApplication.SaveChangesAsync();

            return entity.ToResult();

        }
    }
}
