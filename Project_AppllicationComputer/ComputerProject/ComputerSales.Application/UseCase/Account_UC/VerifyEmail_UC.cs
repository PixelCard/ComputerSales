using ComputerSales.Application.Interface.Account_Interface;
using ComputerSales.Application.Interface.Interface_Email_Respository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.Account_DTO.EmailVerify_DTO;
using System.Text;

namespace ComputerSales.Application.UseCase.Account_UC
{
    public class VerifyEmail_UC
    {
        private readonly IAccountRepository _accounts;
        private readonly IEmailVerifyKeyRepository _keys;
        private readonly IUnitOfWorkApplication _uow;

        public VerifyEmail_UC(IAccountRepository a, IEmailVerifyKeyRepository k, IUnitOfWorkApplication u) { _accounts = a; _keys = k; _uow = u; }

        public async Task Handle(VerifyEmailRequest cmd, CancellationToken ct)
        {
            var acc = await _accounts.GetAccountByID(cmd.AccountId, ct) ?? throw new KeyNotFoundException();
            if (acc.IsLocked()) throw new InvalidOperationException("Tài khoản bị khoá tạm.");

            var hash = Sha256Base64(cmd.RawKey);
            var rec = await _keys.FindAsync(cmd.AccountId, hash, ct) ?? throw new InvalidOperationException("Key không hợp lệ.");
            if (rec.Used) throw new InvalidOperationException("Key đã dùng.");

            if (rec.ExpiresAt < DateTime.UtcNow)
            {
                acc.LockoutFor(TimeSpan.FromSeconds(30));
                await _accounts.UpdateAccount(acc, ct);
                await _uow.SaveChangesAsync(ct);
                throw new InvalidOperationException("Key hết hạn. Khoá tạm 30s.");
            }

            rec.Used = true;
            acc.ConfirmEmail();
            await _keys.UpdateAsync(rec, ct);
            await _accounts.UpdateAccount(acc, ct);
            await _uow.SaveChangesAsync(ct);
        }

        private static string Sha256Base64(string s) { using var sha = System.Security.Cryptography.SHA256.Create(); return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(s))); }
    }
}

