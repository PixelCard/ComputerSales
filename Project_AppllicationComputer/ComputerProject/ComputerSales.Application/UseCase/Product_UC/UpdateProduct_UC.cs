using ComputerSales.Application.Interface.Product_Interface;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.Product_DTO;
using ComputerSales.Application.UseCaseDTO.Product_DTO.UpdateProduct;

namespace ComputerSales.Application.UseCase.Product_UC
{
    public class UpdateProduct_UC
    {
        private readonly IProductRespository _repo;
        private readonly IUnitOfWorkApplication _uow;

        public UpdateProduct_UC(IProductRespository repo, IUnitOfWorkApplication uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<ProductOutputDTOcs?> HandleAsync(UpdateProductInput input, CancellationToken ct = default)
        {
            var entity = await _repo.GetProduct(input.ProductID, ct);
            if (entity is null) return null;

            // set RowVersion = giá trị client gửi (để EF dùng làm OriginalValue khi attach/modify)
            entity.RowVersion = Convert.FromBase64String(input.RowVersionBase64);

            // gán các field được phép cập nhật
            entity.ApplyUpdate(input);

            await _repo.UpdateProduct(entity, ct);

            // ConcurrencyException sẽ ném ra tại đây nếu RowVersion đã thay đổi
            await _uow.SaveChangesAsync(ct);

            return entity.ToResult();
        }
    }
}
