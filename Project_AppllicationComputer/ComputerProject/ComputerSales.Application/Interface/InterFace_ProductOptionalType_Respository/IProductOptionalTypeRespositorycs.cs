using ComputerSales.Domain.Entity.EProduct;

namespace ComputerSales.Application.Interface.InterFace_ProductOptionalType_Respository
{
    public interface IProductOptionalTypeRespositorycs
    {
        Task<ProductOptionType?> GetByTwoIdAsync(long productId, int optionalTypeId, CancellationToken ct);
    }
}
