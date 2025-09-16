using ComputerSales.Application.Interface.Interface_RefreshTokenRespository;
using ComputerSales.Domain.Entity;
using ComputerSales.Domain.Entity.ERefreshToken;
using ComputerSales.Infrastructure.Persistence;
using ComputerSales.Infrastructure.Repositories.UnitOfWork;
using ComputerSales.Infrastructure.Sercurity.JWT.Enity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace ComputerSales.Infrastructure.Repositories.RefreshToken_Respo
{
    public class RefreshTokenRespo : IResfreshTokenRespo
    {
        private readonly AppDbContext _db;
        private readonly JwtOptions _opt;
        private readonly UnitOfWork_Infa unitOfWork_Infa;

        public RefreshTokenRespo(
            AppDbContext db,
            IOptions<JwtOptions> opt,
            UnitOfWork_Infa _unitOfWork_Infa)
        {
            _db = db;
            _opt = opt.Value;
            unitOfWork_Infa= _unitOfWork_Infa;
        }

        // Lấy refresh token còn hiệu lực (RevokedAt == null & chưa hết hạn)
        public async Task<RefreshToken?> GetActiveAsync(string token, CancellationToken ct = default)
        {
            var rt = await _db.RefreshTokens
               .Include(x => x.Account)
               .ThenInclude(a => a.Role)                 // nếu cần role để sinh access token
               .FirstOrDefaultAsync(x => x.Token == token, ct);

            return (rt != null && rt.RevokedAt == null && DateTime.UtcNow < rt.Expires)
                ? rt
                : null;
        }

        // Cấp refresh token mới
        public async Task<RefreshToken> IssueAsync(Account account, CancellationToken ct = default)
        {
            var days = _opt.RefreshTokenDays > 0 ? _opt.RefreshTokenDays : 14;
            var rt = new RefreshToken
            {
                AccountId = account.IDAccount,
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)), // 512-bit
                Expires = DateTime.UtcNow.AddDays(days),
                RevokedAt = null
            };

            _db.RefreshTokens.Add(rt);
            await _db.SaveChangesAsync(ct);
            return rt;
        }


        // Thu hồi 1 refresh token (logout/manual revoke)
        public async Task RevokeAsync(string token, CancellationToken ct = default)
        {
            var rt = await _db.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token, ct);
            if (rt == null) return;

            if (rt.RevokedAt == null)
            {
                rt.RevokedAt = DateTime.UtcNow;
                await _db.SaveChangesAsync(ct);
            }
        }
    }
}
