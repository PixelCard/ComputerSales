// ComputerSales.Infrastructure/Repositories/Account_Respo/AccountRepository.cs
using ComputerSales.Application.Interface.Account_Interface;
using ComputerSales.Domain.Entity;
using ComputerSales.Domain.Entity.EAccount;           // <-- đúng namespace
using ComputerSales.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
namespace ComputerSales.Infrastructure.Repositories.Account_Respo
{
    public class AccountRespository : IAccountRepository
    {
        private readonly AppDbContext _db;
        public AccountRespository(AppDbContext db) => _db = db;

        public async Task AddAccount(Account account, CancellationToken ct = default)
        {
            await _db.Accounts.AddAsync(account, ct);
        }

        public async Task DeleteAccountAsync(int IDAccount,  CancellationToken ct = default)
        {
            // Bỏ filter để tìm cả bản đã đánh dấu xóa (nếu có soft delete)
            var entity = await _db.Accounts
                                  .IgnoreQueryFilters()
                                  .FirstOrDefaultAsync(a => a.IDAccount == IDAccount, ct);
            if (entity is null) return;

            _db.Accounts.Remove(entity);
        }

     

        public Task<Account?> GetAccount(int IDAccount, CancellationToken ct = default)
        {
            // Include tối thiểu; thêm Include(a => a.Role) nếu cần lấy TenRole
            return _db.Accounts
                      .AsNoTracking()
                      //.Include(a => a.Role) // bật nếu cần Role
                      .FirstOrDefaultAsync(a => a.IDAccount == IDAccount, ct);
        }

        public Task UpdateAccount(Account account, CancellationToken ct = default)
        {
            // Gắn entity để EF kiểm tra concurrency (nếu có RowVersion)
            _db.Attach(account);
            _db.Entry(account).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        //lọc theo ID Account
        public Task<Account?> GetAccountById(int IDAccount, CancellationToken ct = default)
        {
            return _db.Accounts
                      .AsNoTracking()
                      .Include(a => a.Role) // nếu cần lấy TenRole
                      .FirstOrDefaultAsync(a => a.IDAccount == IDAccount, ct);
        }

        // ✅ Get by Role
        public Task<Account?> GetAccountByRole(int IDRole, CancellationToken ct = default)
        {
            return _db.Accounts
                      .AsNoTracking()
                      .Include(a => a.Role)
                      .FirstOrDefaultAsync(a => a.IDRole == IDRole, ct);
        }
        public Task<Account?> GetAccountByEmail(string Email, CancellationToken ct = default)
        {
            return _db.Accounts
                      .AsNoTracking()
                      .Include(a => a.Role)
                      .FirstOrDefaultAsync(a => a.Email == Email, ct);
        }
        // Add missing method implementation to satisfy IAccountRepository
        public Task<Account?> GetAccountByID(int IDAccount, CancellationToken ct = default)
        {
            // Implementation matches existing GetAccountById logic
            return _db.Accounts
                      .AsNoTracking()
                      .Include(a => a.Role)
                      .FirstOrDefaultAsync(a => a.IDAccount == IDAccount, ct);
        }



    }
}
