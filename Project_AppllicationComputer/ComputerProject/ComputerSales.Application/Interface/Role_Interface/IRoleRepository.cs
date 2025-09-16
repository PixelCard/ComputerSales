using ComputerSales.Domain.Entity.EAccount;

namespace ComputerSales.Application.Interface.Role_Interface
{
    public interface IRoleRepository
    {
        Task AddRole(Role role, CancellationToken ct);

        Task UpdateRole(Role role, CancellationToken ct);

        Task DeleteRoleAsync(int roleId, byte[]? rowVersion, CancellationToken ct = default);

        Task<Role?> GetRole(int roleId, CancellationToken ct);

        Task<Role?> GetByNameAsync(string name, CancellationToken ct = default);
    }
}
