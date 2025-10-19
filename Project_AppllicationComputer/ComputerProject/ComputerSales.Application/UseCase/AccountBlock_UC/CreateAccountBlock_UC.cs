using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.AccountBlock_DTO;
using ComputerSales.Domain.Entity.EAccount;

namespace ComputerSales.Application.UseCase.AccountBlock_UC
{
    public class CreateAccountBlock_UC
    {
        private readonly IRespository<AccountBlock> _repo;
        private readonly IUnitOfWorkApplication _uow;

        public CreateAccountBlock_UC(IRespository<AccountBlock> repo, IUnitOfWorkApplication uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<AccountBlockOutputDTO> HandleAsync(AccountBlockInputDTO input, CancellationToken ct)
        {
            if (input is null) throw new ArgumentNullException(nameof(input));

            // input.ToEntity() đã chuẩn hoá (và nếu Create() của bạn có UpdateStatusByTime() thì không cần gọi lại)
            var entity = input.ToEntity();

            await _repo.AddAsync(entity, ct);
            await _uow.SaveChangesAsync(ct);

            return entity.ToResult(); // mapper output KHÔNG mutate entity
        }
    }
}
