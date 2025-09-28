using ComputerSales.Application.Interface.Account_Interface;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.Account_DTO;
using ComputerSales.Application.UseCaseDTO.Account_DTO.UpdateAccount;
using ComputerSales.Application.UseCaseDTO.Role_DTO;

namespace ComputerSales.Application.UseCase.Account_UC
{
    public class UpdateAccount_UC
    {
        private readonly IAccountRepository _repo;
        private readonly IUnitOfWorkApplication _uow;

        public UpdateAccount_UC(IAccountRepository repo, IUnitOfWorkApplication uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<AccountOutputDTO?> HandleAsync(int Id,UpdateAccountDTO input, CancellationToken ct = default)
        {
            var entity = await _repo.GetAccountByID(Id, ct);
            if (entity is null) return null;

            // nếu dùng concurrency
            // if (!string.IsNullOrWhiteSpace(input.RowVersionBase64))
            //     entity.RowVersion = Convert.FromBase64String(input.RowVersionBase64);

            entity.Email = input.Email.Trim();


            await _repo.UpdateAccount(entity, ct);

            await _uow.SaveChangesAsync(ct);

            var entity_SauKhiSua = await _repo.GetAccountByID(Id, ct);

            return entity_SauKhiSua.ToResult();
        }

    }

}
