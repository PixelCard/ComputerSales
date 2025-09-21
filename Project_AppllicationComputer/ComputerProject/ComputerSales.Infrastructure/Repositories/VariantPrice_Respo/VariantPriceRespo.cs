using ComputerSales.Application.Interface.Interface_VariantPriceRespo;
using ComputerSales.Application.UseCaseDTO.VariantPrice_DTO;
using ComputerSales.Domain.Entity.EVariant;
using ComputerSales.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ComputerSales.Infrastructure.Repositories.VariantPrice_Respo
{
    public class VariantPriceRespo : IVariantPriceRespo
    {
        private AppDbContext dbContext;

        public VariantPriceRespo(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<VariantPrice> variantGetPriceByVariantID(int idVariant , CancellationToken ct)
            => dbContext.variantPrices.AsNoTracking().FirstOrDefaultAsync(x => x.VariantId == idVariant);
    }
}
