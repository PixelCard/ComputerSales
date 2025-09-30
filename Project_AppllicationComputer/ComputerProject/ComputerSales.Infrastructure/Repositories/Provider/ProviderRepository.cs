using ComputerSales.Domain.Entity.EProvider;
using ComputerSales.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ComputerSales.Infrastructure.Repositories
{
    public class ProviderRepository
    {
        private readonly AppDbContext _db;

        public ProviderRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Provider>> GetAllProvidersAsync(CancellationToken ct = default)
        {
            return await _db.Providers
                            .AsNoTracking()
                            .OrderBy(p => p.ProviderName)
                            .ToListAsync(ct);
        }
    }

}
