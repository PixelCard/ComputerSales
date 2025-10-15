using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.AccountBlock_DTO;
using ComputerSales.Application.UseCaseDTO.AccountBlock_DTO.DeleteAccountBlock;
using ComputerSales.Domain.Entity.EAccount;

namespace ComputerSales.Application.UseCase.AccountBlockDTO
{
    public class DeleteAccountBlock_UC
    {
        private IRespository<AccountBlock> respository;

        private IUnitOfWorkApplication unitOfWorkApplication;

        public DeleteAccountBlock_UC(IRespository<AccountBlock> respository,
            IUnitOfWorkApplication unitOfWorkApplication)
        {
            this.respository = respository;
            this.unitOfWorkApplication = unitOfWorkApplication;
        }

        public async Task<bool> HandleAsync(DeleteAccountBlockInput_DTO input, CancellationToken ct)
        {
            var entity = await respository.GetByIdAsync(input.IdBlock, ct);
            if (entity == null) return false;

            respository.Remove(entity);
            await unitOfWorkApplication.SaveChangesAsync(ct);
            return true;
        }

    }
}
