using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.UseCaseDTO.AccountBlock_DTO;
using ComputerSales.Domain.Entity.EAccount;

namespace ComputerSales.Application.UseCase.AccountBlock_UC
{
    public sealed class GetAllAccountBlock_UC
    {
        private readonly IRespository<AccountBlock> _repo;
        public GetAllAccountBlock_UC(IRespository<AccountBlock> repo) => _repo = repo;

        public async Task<List<AccountBlockOutputDTO>> HandleAsync(CancellationToken ct = default)
        {
            var list = await _repo.ListAsync(ct: ct);
            return list.Select(e => e.ToResult()).ToList();
        }
    }
}
