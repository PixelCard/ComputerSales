using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.ProductProtection_DTO;
using ComputerSales.Application.UseCaseDTO.ProductVariant_DTO;
using ComputerSales.Application.UseCaseDTO.ProductVariant_DTO.UpdateDTO;
using ComputerSales.Domain.Entity.EProduct;

namespace ComputerSales.Application.UseCase.ProductVariant_UC
{
    public class UpdateProductVariant_UC
    {
        private IRespository<ProductVariant> _repo;
        private IUnitOfWorkApplication _unitOfWorkApplication;

        public UpdateProductVariant_UC(IRespository<ProductVariant> repoProductVariant,
            IUnitOfWorkApplication unitOfWorkApplication)
        {
            _repo = repoProductVariant;
            _unitOfWorkApplication = unitOfWorkApplication;
        }

        public async Task<ProductVariantOutput?> HandleAsync(UpdateDTO_ProductVariant input, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(input.Id, ct);

            if (entity == null) return null;

            entity.Quantity = input.Quantity;
            entity.ProductId = input.ProductId;

            entity.SKU = input.SKU;

            entity.Status = input.Status;

            entity.VariantName = input.VariantName;

            _repo.Update(entity);

            await _unitOfWorkApplication.SaveChangesAsync(ct);

            return entity.ToResult();
        }
    }
}
