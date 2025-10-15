using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Domain.Entity.EAccount;

namespace ComputerSales.Application.AccountBlockDTO
{
    public class CheckAccountBlock_UC
    {
        private readonly IRespository<AccountBlock> _repository;

        public CheckAccountBlock_UC(IRespository<AccountBlock> repository)
        {
            _repository = repository;
        }

        public async Task<AccountBlock?> HandleAsync(int accountId, CancellationToken ct = default)
        {
            // Lấy tất cả AccountBlock của tài khoản này
            var blocks = await _repository.ListAsync(ct: ct);

            // Tìm block đang active (đang trong thời gian khóa)
            var activeBlock = blocks
                .Where(x => x.IDAccount == accountId)
                .FirstOrDefault(x => x.IsActiveNowUtc);

            if (activeBlock != null)
            {
                // Cập nhật trạng thái để đảm bảo chính xác
                activeBlock.UpdateStatusByTime();
                return activeBlock;
            }

            return null;
        }
    }
}
