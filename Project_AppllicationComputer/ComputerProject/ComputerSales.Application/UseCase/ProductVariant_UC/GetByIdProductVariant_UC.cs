using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.ProductProtection_DTO;
using ComputerSales.Application.UseCaseDTO.ProductVariant_DTO;
using ComputerSales.Application.UseCaseDTO.ProductVariant_DTO.GetByIdDTO;
using ComputerSales.Domain.Entity.EProduct;

namespace ComputerSales.Application.UseCase.ProductVariant_UC
{
    public class GetByIdProductVariant_UC
    {
        private IRespository<ProductVariant> _repo;
        private IUnitOfWorkApplication _unitOfWorkApplication;

        public GetByIdProductVariant_UC(IRespository<ProductVariant> repo, 
            IUnitOfWorkApplication unitOfWorkApplication)
        {
            _repo = repo;
            _unitOfWorkApplication = unitOfWorkApplication;
        }

        public async Task<ProductVariantOutput?> HandleAsync(GetById_ProductVariant_DTOcs input, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(input.id, ct);

            if (entity == null) return null;

            return entity.ToResult();
        }
    }
}




