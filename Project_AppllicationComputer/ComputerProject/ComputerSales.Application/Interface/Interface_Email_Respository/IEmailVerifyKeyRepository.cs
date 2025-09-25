using ComputerSales.Domain.Entity.EAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.Interface.Interface_Email_Respository
{
    public interface IEmailVerifyKeyRepository
    {
        Task AddAsync(EmailVerifyKey key, CancellationToken ct);
        Task<EmailVerifyKey?> FindAsync(int accountId, string keyHash, CancellationToken ct);
        Task UpdateAsync(EmailVerifyKey key, CancellationToken ct);

        Task CleanUpExpiredKeys(int accountId, CancellationToken ct);

        Task<List<EmailVerifyKey>> GetKeysByAccountId(int accountId, CancellationToken ct);
    }
}
