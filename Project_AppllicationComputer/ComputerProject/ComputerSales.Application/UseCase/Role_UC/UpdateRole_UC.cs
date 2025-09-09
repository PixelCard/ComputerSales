using ComputerSales.Application.Interface.Role_Interface;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.Role_DTO;
using ComputerSales.Application.UseCaseDTO.Role_DTO.UpdateRole;

namespace ComputerSales.Application.UseCase.Role_UC
{
    public class UpdateRole_UC
    {
        private readonly IRoleRepository _repo;
        private readonly IUnitOfWorkApplication _uow;

        public UpdateRole_UC(IRoleRepository repo, IUnitOfWorkApplication uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<RoleOutputDTO?> HandleAsync(UpdateRoleDTO input, CancellationToken ct = default)
        {
            var entity = await _repo.GetRole(input.IDRole, ct);
            if (entity is null) return null;

            // nếu dùng concurrency
            // if (!string.IsNullOrWhiteSpace(input.RowVersionBase64))
            //     entity.RowVersion = Convert.FromBase64String(input.RowVersionBase64);

            entity.TenRole = input.TenRole.Trim();   // hoặc entity.ApplyUpdate(input);

            await _repo.UpdateRole(entity, ct);
            await _uow.SaveChangesAsync(ct);

            return entity.ToResult();
        }

    }
}
