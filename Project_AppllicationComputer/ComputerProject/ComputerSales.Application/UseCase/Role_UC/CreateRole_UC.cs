using ComputerSales.Application.Interface.Role_Interface;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.Role_DTO;
using ComputerSales.Domain.Entity.EAccount; // Role entity

namespace ComputerSales.Application.UseCase.Role_UC
{
    public class CreateRole_UC
    {
        private readonly IRoleRepository _repo;
        private readonly IUnitOfWorkApplication _uow;

        public CreateRole_UC(IRoleRepository repo, IUnitOfWorkApplication uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<RoleOutputDTO> HandleAsync(RoleDTOInput input, CancellationToken ct = default)
        {
            // (tuỳ bạn) validate: TenRole không rỗng, check trùng tên, v.v.

            Role entity = input.ToEnity(); // dùng mapping extension

            await _repo.AddRole(entity, ct);

            await _uow.SaveChangesAsync(ct);

            return entity.ToResult();
        }
    }
}
