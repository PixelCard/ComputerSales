using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.AccountBlock_DTO;
using ComputerSales.Domain.Entity.EAccount;

namespace ComputerSales.Application.UseCase.AccountBlockDTO
{
    public class CreateAccountBlock_UC
    {
        private IRespository<AccountBlock> respository;

        private IUnitOfWorkApplication unitOfWorkApplication;

        public CreateAccountBlock_UC(IRespository<AccountBlock> respository,
            IUnitOfWorkApplication unitOfWorkApplication)
        {
            this.respository = respository;
            this.unitOfWorkApplication = unitOfWorkApplication;
        }

        public async Task<AccountBlockOutputDTO?> HandleAsync(AccountBlockInputDTO input, CancellationToken ct)
        {
            AccountBlock entity = input.ToEntity();

            if (entity == null)
            {
                return null;
            }

            await respository.AddAsync(entity, ct);

            await unitOfWorkApplication.SaveChangesAsync(ct);

            return entity.ToResult();
        }
    }
}
