using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.UseCaseDTO.AccountBlock_DTO;
using ComputerSales.Domain.Entity.EAccount;

namespace ComputerSales.Application.UseCase.AccountBlock_UC
{
    public  class CheckAccountBlock_UC
    {
        private readonly IRespository<AccountBlock> _repo;
        public CheckAccountBlock_UC(IRespository<AccountBlock> repo) => _repo = repo;

        // Trả về DTO hoặc null nếu không bị block
        public async Task<AccountBlockOutputDTO?> HandleAsync(int accountId, CancellationToken ct = default)
        {
            var now = DateTime.UtcNow;

            // Nếu repo của bạn có AnyAsync/FirstOrDefaultAsync theo predicate thì dùng;
            // nếu chưa có, thêm overload hoặc Specification để tránh ListAsync() toàn bảng.
            var activeBlock = await _repo.FirstOrDefaultAsync(
                x => x.IDAccount == accountId
                  && x.BlockFromUtc <= now
                  && (!x.BlockToUtc.HasValue || now < x.BlockToUtc.Value),
                ct
            );

            return activeBlock?.ToResult(); // ToResult() KHÔNG mutate entity
        }
    }
}
