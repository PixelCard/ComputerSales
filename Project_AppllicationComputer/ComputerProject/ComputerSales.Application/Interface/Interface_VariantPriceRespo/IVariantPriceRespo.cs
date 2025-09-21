using ComputerSales.Application.UseCaseDTO.VariantPrice_DTO;
using ComputerSales.Domain.Entity.EVariant;

namespace ComputerSales.Application.Interface.Interface_VariantPriceRespo
{
    public interface IVariantPriceRespo
    {
        Task<VariantPrice> variantGetPriceByVariantID(int idVariant, CancellationToken ct);
    }
}
