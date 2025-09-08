using ComputerSales.Application.Interface.Role_Interface;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.Role_DTO.DeleteRole;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCase.Role_UC
{
    public class DeleteRole_UC
    {
        private readonly IRoleRepository _repo;
        private readonly IUnitOfWorkApplication _uow;

        public DeleteRole_UC(IRoleRepository repo, IUnitOfWorkApplication uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<bool> HandleAsync(DeleteRoleInput input, CancellationToken ct = default)
        {
            byte[]? rv = null;

            //if (!string.IsNullOrWhiteSpace(input.RowVersionBase64))
            //    rv = Convert.FromBase64String(input.RowVersionBase64!);

            await _repo.DeleteRoleAsync(input.IDRole, rv, ct);

            // nếu không tìm thấy entity thì repo có thể không làm gì => coi như xóa thất bại
            var changes = await _uow.SaveChangesAsync(ct);
            return changes > 0;
        }
    }
}
