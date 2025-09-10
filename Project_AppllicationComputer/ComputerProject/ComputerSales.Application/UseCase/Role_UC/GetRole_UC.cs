using ComputerSales.Application.Interface.Role_Interface;
using ComputerSales.Application.UseCaseDTO.Role_DTO;
using ComputerSales.Application.UseCaseDTO.Role_DTO.GetRoleByID;

namespace ComputerSales.Application.UseCase.Role_UC
{
    public class GetRole_UC
    {
        private readonly IRoleRepository _repo;
        public GetRole_UC(IRoleRepository repo) => _repo = repo;

        public async Task<RoleOutputDTO?> HandleAsync(GetRoleByID input, CancellationToken ct = default)
        {
            var entity = await _repo.GetRole(input.IDRole, ct); // OK vì GetRoleByID có IDRole
            if (entity is null) return null;
            return entity.ToResult();
        }
    }
}
