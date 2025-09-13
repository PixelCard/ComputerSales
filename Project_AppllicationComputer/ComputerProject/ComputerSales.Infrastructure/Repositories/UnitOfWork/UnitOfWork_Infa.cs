using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Storage;

namespace ComputerSales.Infrastructure.Repositories.UnitOfWork
{
    public class UnitOfWork_Infa : IUnitOfWorkApplication
    {
        private readonly AppDbContext _db;

        private IDbContextTransaction? _tx;

        public UnitOfWork_Infa(AppDbContext db) => _db = db;

        public async Task BeginTransactionAsync(CancellationToken ct = default)
        {
            if (_tx is null)
                _tx = await _db.Database.BeginTransactionAsync(ct);
        }

        public async Task CommitAsync(CancellationToken ct = default)
        {
            if (_tx is not null)
            {
                await _tx.CommitAsync(ct);
                await _tx.DisposeAsync();
                _tx = null;
            }
        }

        public async Task RollbackAsync(CancellationToken ct = default)
        {
            if (_tx is not null)
            {
                await _tx.RollbackAsync(ct);
                await _tx.DisposeAsync();
                _tx = null;
            }
        }

        public Task<int> SaveChangesAsync(CancellationToken ct = default) => _db.SaveChangesAsync(ct); //Sau khi gọi hàm này thì data mới vào db
    }
}
