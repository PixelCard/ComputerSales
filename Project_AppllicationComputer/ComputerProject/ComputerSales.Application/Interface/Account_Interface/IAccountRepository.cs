using ComputerSales.Domain.Entity;

namespace ComputerSales.Application.Interface.Account_Interface
{
    public interface IAccountRepository
    {
        Task AddAccount(Account account, CancellationToken ct);

        Task UpdateAccount(Account account, CancellationToken ct);

        Task DeleteAccountAsync(int IDAccount, CancellationToken ct = default);

        Task<bool> ExistsByEmailAsync(string Email, CancellationToken ct = default);

        // Cho phép null vì có thể không tìm thấy
        Task<Account?> GetAccount(int IDAccount, CancellationToken ct); // lấy theo ID
        Task<Account?> GetAccountByRole(int IDRole, CancellationToken ct = default);
        Task<Account?> GetAccountByID(int IDAccount, CancellationToken ct = default);
        Task<Account?> GetAccountByEmail(string Email, CancellationToken ct = default);
    }
}
