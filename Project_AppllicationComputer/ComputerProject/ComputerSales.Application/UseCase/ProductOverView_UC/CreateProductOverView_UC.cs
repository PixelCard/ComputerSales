using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.ProductOverView_DTO;
using ComputerSales.Domain.Entity.EProduct;

namespace ComputerSales.Application.UseCase.ProductOvetView_UC
{
    public class CreateProductOverView_UC
    {
        private IRespository<ProductOverview> _repoProductOverView;
        private IUnitOfWorkApplication _unitOfWorkApplication;

        public CreateProductOverView_UC(IRespository<ProductOverview> productOverView, 
            IUnitOfWorkApplication unitOfWorkApplication)
        {
            _repoProductOverView = productOverView;
            _unitOfWorkApplication = unitOfWorkApplication;
        }

        public async Task<ProductOverViewOutput> HandleAsync(ProductOverViewInput input,CancellationToken ct)
        {
            ProductOverview productOverView=input.ToEntity();

            await _repoProductOverView.AddAsync(productOverView, ct);

            await _unitOfWorkApplication.SaveChangesAsync();

            return productOverView.ToResult();
        }
    }
}
