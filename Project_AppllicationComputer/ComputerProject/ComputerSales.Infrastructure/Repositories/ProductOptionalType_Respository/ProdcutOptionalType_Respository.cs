using ComputerSales.Application.Interface.InterFace_ProductOptionalType_Respository;
using ComputerSales.Domain.Entity.EProduct;
using ComputerSales.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ComputerSales.Infrastructure.Repositories.ProductOptionalType_Respository
{
    public class ProdcutOptionalType_Respository : IProductOptionalTypeRespositorycs
    {
        private AppDbContext _dbContext;
public ProdcutOptionalType_Respository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ProductOptionType?> GetByTwoIdAsync(long productId, int optionalTypeId, CancellationToken ct)
        {
            return await _dbContext.
                productOptionTypes.
                FirstOrDefaultAsync(x => x.ProductId == productId && x.OptionTypeId == optionalTypeId, ct);
        }
    }
}
