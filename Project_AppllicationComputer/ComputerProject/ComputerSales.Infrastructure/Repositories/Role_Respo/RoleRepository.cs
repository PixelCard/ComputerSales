using ComputerSales.Application.Interface.Role_Interface;
using ComputerSales.Domain.Entity.EAccount;
using ComputerSales.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace ComputerSales.Infrastructure.Repositories.Role_Respo
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _db;

        public RoleRepository(AppDbContext db) => _db = db;

        public async Task DeleteRoleAsync(int roleId, byte[]? rowVersion, CancellationToken ct = default)
        {
            // Bỏ filter để tìm cả bản đã đánh dấu xóa (nếu có soft delete)
            var entity = await _db.Roles
                                  .IgnoreQueryFilters()
                                  .FirstOrDefaultAsync(r => r.IDRole == roleId, ct);

            if (entity is null) return;

            //if (rowVersion is not null && rowVersion.Length > 0)
            //{
            //    // Concurrency: đặt OriginalValue để EF sinh WHERE RowVersion = @original
            //    _db.Entry(entity).Property(e => e.RowVersion).OriginalValue = rowVersion;
            //}

            _db.Roles.Remove(entity);
        }

        public async Task AddRole(Role role, CancellationToken ct = default)
        {
            await _db.Roles.AddAsync(role, ct);
        }

        public Task UpdateRole(Role role, CancellationToken ct = default)
        {
            _db.Attach(role);
            _db.Entry(role).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public Task<Role?> GetRole(int IDRole, CancellationToken ct = default)
        {
            return _db.Roles
                      .AsNoTracking()
                      .FirstOrDefaultAsync(r => r.IDRole == IDRole, ct);
        }
    }
}
