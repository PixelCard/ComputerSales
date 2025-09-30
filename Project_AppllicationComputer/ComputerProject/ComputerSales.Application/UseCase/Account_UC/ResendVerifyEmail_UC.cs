using ComputerSales.Application.Interface.Account_Interface;
using ComputerSales.Application.Interface.Interface_Email_Respository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.Account_DTO.ResendVerifyEmaiDTO;
using ComputerSales.Domain.Entity.EAccount;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCase.Account_UC
{
    public class ResendVerifyEmail_UC
    {
        private readonly IAccountRepository _accounts;
        private readonly IEmailVerifyKeyRepository _keys;
        private readonly IEmailSender _email;
        private readonly IUnitOfWorkApplication _uow;
        private readonly IConfiguration _cfg;

        public ResendVerifyEmail_UC(IAccountRepository a, IEmailVerifyKeyRepository k, IEmailSender e, IUnitOfWorkApplication u, IConfiguration c)
        { _accounts = a; _keys = k; _email = e; _uow = u; _cfg = c; }

        public async Task Handle(ResendVerifyEmailDTO cmd, CancellationToken ct)
        {
            var acc = await _accounts.GetAccountByID(cmd.AccountId, ct) ?? throw new KeyNotFoundException();

            if (acc.EmailConfirmed) return;

            if (acc.IsLocked()) throw new InvalidOperationException("Đang bị khoá tạm.");

            if (acc.VerifyKeyExpiresAt.HasValue && acc.VerifyKeyExpiresAt.Value > DateTime.UtcNow)
                throw new InvalidOperationException("Liên kết hiện tại chưa hết hạn.");

            // Kiểm tra thời gian hết hạn của verify key
            if (acc.VerifyKeyExpiresAt.HasValue && acc.VerifyKeyExpiresAt.Value > DateTime.UtcNow)
                throw new InvalidOperationException("Liên kết xác thực email hiện tại chưa hết hạn.");

            // rate limit: 5 lần/ngày
            acc.BumpSendCount();
            if (acc.VerifySendCountToday > 5) throw new InvalidOperationException("Vượt giới hạn gửi lại.");

            // Lấy tất cả các key của người dùng và xóa các key đã hết hạn hoặc chưa được sử dụng
            await _keys.CleanUpExpiredKeys(cmd.AccountId, ct);

            var (raw, rec, expireAt) = CreateVerifyKey(acc.IDAccount, 60);
            acc.MarkVerifyWindow(expireAt);

            await _uow.BeginTransactionAsync(ct);
            await _keys.AddAsync(rec, ct);
            await _accounts.UpdateAccount(acc, ct);
            await _uow.SaveChangesAsync(ct);

            //Production(Public)

            //var link2 = $"{_cfg["ngrok:BaseUrl"]}/account/verify?uid={acc.IDAccount}&key={Uri.EscapeDataString(raw)}";
            

            //Local(LocalHost)
            var link = $"{_cfg["Frontend:BaseUrl"]}/account/verify?uid={acc.IDAccount}&key={Uri.EscapeDataString(raw)}";


            var html = $@"<p>Liên kết mới (hết hạn 60 giây): <a href=""{link}"">Xác thực email</a></p>";
            await _email.SendAsync(acc.Email, "Gửi lại xác thực email", html, ct);

            await _uow.CommitAsync(ct);
        }

        private static (string raw, EmailVerifyKey rec, DateTime expireAt) CreateVerifyKey(int accountId, int seconds)
        {
            var raw = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)).TrimEnd('=').Replace('+', '-').Replace('/', '_');
            using var sha = SHA256.Create();
            var hash = Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(raw)));
            var expireAt = DateTime.UtcNow.AddSeconds(seconds);
            return (raw, new EmailVerifyKey { Id = Guid.NewGuid(), AccountId = accountId, KeyHash = hash, ExpiresAt = expireAt }, expireAt);
        }
    }
}
