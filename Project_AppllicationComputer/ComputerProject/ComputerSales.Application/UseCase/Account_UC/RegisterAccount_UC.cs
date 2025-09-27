using ComputerSales.Application.Interface.Account_Interface;
using ComputerSales.Application.Interface.Interface_Email_Respository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCase.Customer_UC;
using ComputerSales.Application.UseCaseDTO.Account_DTO.RegisterDTO;
using ComputerSales.Domain.Entity;
using ComputerSales.Domain.Entity.EAccount;
using ComputerSales.Domain.Entity.ECustomer;
using Microsoft.Extensions.Configuration;
using ComputerSales.Application.UseCaseDTO.Customer_DTO;
using System.Security.Cryptography;
using System.Text;
using ComputerSales.Application.Interface.InterfaceRespository;

namespace ComputerSales.Application.UseCase.Account_UC
{
    public class RegisterAccount_UC
    {
        private readonly IAccountRepository _accounts;
        private readonly IRespository<Customer> _customer;
        private readonly IEmailVerifyKeyRepository _keys;
        private readonly IUnitOfWorkApplication _uow;
        private readonly IEmailSender _email;
        private readonly IConfiguration _cfg;

        public RegisterAccount_UC(
            IAccountRepository accounts, IEmailVerifyKeyRepository keys,
            IUnitOfWorkApplication uow, IEmailSender email, IConfiguration cfg,
            IRespository<Customer> customer)
        { _accounts = accounts; _keys = keys; _uow = uow; _email = email; _cfg = cfg; _customer = customer; }

        public async Task Handle(RegisterRequestDTO cmd, CancellationToken ct)
        {
            var emailNorm = cmd.Email.Trim().ToLowerInvariant();
            if (await _accounts.GetAccountByEmail(emailNorm, ct) != null)
                throw new InvalidOperationException("Email đã tồn tại.");

            var hash = BCrypt.Net.BCrypt.HashPassword(cmd.Password);
            var acc = Account.Create(emailNorm, hash, cmd.RoleId ?? 1,DateTime.Now);

            // map Customer nếu cần…

            await _uow.BeginTransactionAsync(ct);

            await _accounts.AddAccount(acc, ct);

            await _uow.SaveChangesAsync(ct);

            var customer = Customer.create(null, cmd.UserName, cmd.Description_User, cmd.address, cmd.phone, DateTime.Now, acc.IDAccount);

            await _customer.AddAsync(customer,ct);

            await _uow.SaveChangesAsync(ct);

            // tạo verify key 60s
            var (rawKey, rec, expireAt) = CreateVerifyKey(acc.IDAccount, seconds: 60);
            acc.MarkVerifyWindow(expireAt);
            await _keys.AddAsync(rec, ct);
            await _accounts.UpdateAccount(acc, ct);
            await _uow.SaveChangesAsync(ct);

            // gửi email
            var link = $"{_cfg["Frontend:BaseUrl"]}/Account/Verify?uid={acc.IDAccount}&key={Uri.EscapeDataString(rawKey)}";
            var html = $@"<p>Nhấn để xác thực (hết hạn sau 60 giây): <a href=""{link}"">Xác thực email</a></p>";
            await _email.SendAsync(acc.Email, "Xác nhận email", html, ct);

            await _uow.CommitAsync(ct);
        }

        private static (string raw, EmailVerifyKey rec, DateTime expireAt) CreateVerifyKey(int accountId, int seconds)
        {
            var raw = NewBase64Url(32);
            var hash = Sha256Base64(raw);
            var expireAt = DateTime.UtcNow.AddSeconds(seconds);
            return (raw, new EmailVerifyKey { Id = Guid.NewGuid(), AccountId = accountId, KeyHash = hash, ExpiresAt = expireAt }, expireAt);
        }

        // utils (có thể đưa vào Common)
        private static string NewBase64Url(int bytes) { using var rng = RandomNumberGenerator.Create(); var b = new byte[bytes]; rng.GetBytes(b); return Convert.ToBase64String(b).TrimEnd('=').Replace('+', '-').Replace('/', '_'); }
        private static string Sha256Base64(string s) { using var sha = SHA256.Create(); return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(s))); }
    }
}
