using ComputerSales.Application.Interface.Interface_Email_Respository;
using ComputerSales.Domain.Entity.EAccount;
using ComputerSales.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Infrastructure.Repositories.EmailVerifyKeyRepository
{

    public sealed class EmailVerifyKeyRepository : IEmailVerifyKeyRepository
    {
        private readonly AppDbContext _db;

        public EmailVerifyKeyRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(EmailVerifyKey key, CancellationToken ct)
           => await _db.EmailVerifyKeys.AddAsync(key, ct);

        public async Task CleanUpExpiredKeys(int accountId, CancellationToken ct)
        {
            var expiredKeys = await GetKeysByAccountId(accountId, ct);
            var unusedKeys = expiredKeys.Where(k => !k.Used && k.ExpiresAt < DateTime.UtcNow).ToList();

            if (unusedKeys.Any())
            {
                _db.EmailVerifyKeys.RemoveRange(unusedKeys);
                await _db.SaveChangesAsync(ct);
            }
        }

        public async Task<EmailVerifyKey?> FindAsync(int accountId, string keyHash, CancellationToken ct)
             => await _db.EmailVerifyKeys
                        .FirstOrDefaultAsync(x => x.AccountId == accountId && x.KeyHash == keyHash, ct);

        public async Task<List<EmailVerifyKey>> GetKeysByAccountId(int accountId, CancellationToken ct)
        {
            // Truy vấn tất cả EmailVerifyKey của tài khoản với AccountId
            return await _db.EmailVerifyKeys.Where(key => key.AccountId == accountId).ToListAsync(ct);
        }

        public Task UpdateAsync(EmailVerifyKey key, CancellationToken ct)
        {
            _db.EmailVerifyKeys.Update(key);
            return Task.CompletedTask;
        }


    }
}
