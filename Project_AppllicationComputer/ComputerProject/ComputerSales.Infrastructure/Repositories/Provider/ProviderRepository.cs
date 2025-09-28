using ComputerSales.Application.UseCaseDTO.Provider_DTO; // dùng ProviderOutput
using ComputerSales.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ComputerSales.Infrastructure.Repositories
{
    public sealed class ProviderRepository
    {
        private readonly AppDbContext _db;

        public ProviderRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<ProviderOutput>> GetAllProvidersAsync(CancellationToken ct = default)
        {
            return await _db.Providers
                .AsNoTracking()
                .OrderBy(p => p.ProviderName)
                .Select(p => new ProviderOutput(
                    p.ProviderID,
                    p.ProviderName
                ))
                .ToListAsync(ct);
        }

    }
}
