// ComputerSales.Application/UseCase/Account_UC/DeleteAccount_UC.cs
using ComputerSales.Application.Interface.Account_Interface;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.Account_DTO.DeleteAccount;

namespace ComputerSales.Application.UseCase.Account_UC
{
    public class DeleteAccount_UC
    {
        private readonly IAccountRepository _repo;
        private readonly IUnitOfWorkApplication _uow;

        public DeleteAccount_UC(IAccountRepository repo, IUnitOfWorkApplication uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<bool> HandleAsync(DeleteAccountOutputDTO input, CancellationToken ct = default)
        { 
            await _repo.DeleteAccountAsync(input.IDAccount, ct);

            var changes = await _uow.SaveChangesAsync(ct);
            return changes > 0;
        }
    }
}
