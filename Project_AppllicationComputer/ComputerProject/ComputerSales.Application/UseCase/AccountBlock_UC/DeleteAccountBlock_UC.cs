using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.AccountBlock_DTO.DeleteAccountBlock;
using ComputerSales.Domain.Entity.EAccount;

namespace ComputerSales.Application.UseCase.AccountBlock_UC
{
    public sealed class DeleteAccountBlock_UC
    {
        private readonly IRespository<AccountBlock> _repo;
        private readonly IUnitOfWorkApplication _uow;

        public DeleteAccountBlock_UC(IRespository<AccountBlock> repo, IUnitOfWorkApplication uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<bool> HandleAsync(DeleteAccountBlockInput_DTO input, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(input.BlockId, ct);
            if (entity is null) return false;

            _repo.Remove(entity);
            await _uow.SaveChangesAsync(ct);
            return true;
        }
    }
}
