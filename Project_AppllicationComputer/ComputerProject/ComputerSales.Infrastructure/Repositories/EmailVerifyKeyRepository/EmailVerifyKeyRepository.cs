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

        public async Task<EmailVerifyKey?> FindAsync(int accountId, string keyHash, CancellationToken ct)
             => await _db.EmailVerifyKeys
                        .FirstOrDefaultAsync(x => x.AccountId == accountId && x.KeyHash == keyHash, ct);

        public Task UpdateAsync(EmailVerifyKey key, CancellationToken ct)
        {
            _db.EmailVerifyKeys.Update(key);
            return Task.CompletedTask;
        }
    }
}
