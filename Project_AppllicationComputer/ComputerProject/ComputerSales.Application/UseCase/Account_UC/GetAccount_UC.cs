// ComputerSales.Application/UseCase/Account_UC/GetAccount_UC.cs
using ComputerSales.Application.Interface.Account_Interface;
using ComputerSales.Application.UseCaseDTO.Account_DTO;
using ComputerSales.Application.UseCaseDTO.Account_DTO.GetAccountByID;


namespace ComputerSales.Application.UseCase.Account_UC
{
    public class GetAccount_UC
    {
        private readonly IAccountRepository _repo;
        public GetAccount_UC(IAccountRepository repo) => _repo = repo;

        public async Task<AccountOutputDTO?> HandleAsync(getAccountByID input, CancellationToken ct = default)
        {
            var entity = await _repo.GetAccountByID(input.IDAccount, ct);
            if (entity is null) return null;

            return entity.ToResult(); // mapping sang DTO có TenRole
        }
    }
}
