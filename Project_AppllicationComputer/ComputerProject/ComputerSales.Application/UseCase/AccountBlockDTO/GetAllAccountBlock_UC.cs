using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.UseCaseDTO.AccountBlock_DTO;
using ComputerSales.Domain.Entity.EAccount;

namespace ComputerSales.Application.UseCase.AccountBlockDTO
{
    public class GetAllAccountBlock_UC
    {
        private readonly IRespository<AccountBlock> _repo;

        public GetAllAccountBlock_UC(IRespository<AccountBlock> repo)
        {
            _repo = repo;
        }

        public async Task<List<AccountBlockOutputDTO>> HandleAsync(CancellationToken ct = default)
        {
            // Lấy danh sách toàn bộ bản ghi AccountBlock
            var list = await _repo.ListAsync(ct: ct);

            // Chuyển entity → DTO (sử dụng mapping ToResult())
            return list.Select(ab => ab.ToResult()).ToList();
        }
    }
}
