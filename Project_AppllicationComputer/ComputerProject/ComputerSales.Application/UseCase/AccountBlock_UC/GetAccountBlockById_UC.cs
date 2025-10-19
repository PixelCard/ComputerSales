// ComputerSales.Application/UseCase/AccountBlock_UC/GetAccountBlockById_UC.cs
using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.UseCaseDTO.AccountBlock_DTO;
using ComputerSales.Application.UseCaseDTO.AccountBlock_DTO.GetAccountBlock;
using ComputerSales.Domain.Entity.EAccount;

namespace ComputerSales.Application.UseCase.AccountBlock_UC
{
    public class GetAccountBlockById_UC
    {
        private readonly IRespository<AccountBlock> _repo;
        public GetAccountBlockById_UC(IRespository<AccountBlock> repo) => _repo = repo;

        public async Task<AccountBlockOutputDTO?> HandleAsync(GetAccountBlockByID_InputDTO input, CancellationToken ct)
        {
            var e = await _repo.GetByIdAsync(input.BlockId, ct);
            return e?.ToResult();
        }
    }
}
