using ComputerSales.Domain.Entity;
using ComputerSales.Domain.Entity.ERefreshToken;

namespace ComputerSales.Application.Interface.Interface_RefreshTokenRespository
{
    public interface IResfreshTokenRespo
    {
        Task<RefreshToken> IssueAsync(Account account, CancellationToken ct = default);
        Task<RefreshToken?> GetActiveAsync(string token, CancellationToken ct = default);
        Task RevokeAsync(string token, CancellationToken ct = default);
    }
}
