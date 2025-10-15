using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.AccountBlock_DTO;
using ComputerSales.Application.UseCaseDTO.AccountBlock_DTO.GetAccountBlock;
using ComputerSales.Application.UseCaseDTO.Category_DTO;
using ComputerSales.Domain.Entity.EAccount;
using ComputerSales.Domain.Entity.ECategory;

namespace ComputerSales.Application.UseCase.AccountBlockDTO
{
    public class getAccountBlockByID_UC
    {
        private IRespository<AccountBlock> respository;

        private IUnitOfWorkApplication unitOfWorkApplication;

        public getAccountBlockByID_UC(IRespository<AccountBlock> respository,
            IUnitOfWorkApplication unitOfWorkApplication)
        {
            this.respository = respository;
            this.unitOfWorkApplication = unitOfWorkApplication;
        }

        public async Task<AccountBlockOutputDTO?> HandleAsync(GetAccountBlockByID_InputDTO input, CancellationToken ct)
        {
            var entity = await respository.GetByIdAsync(input.IdBlock, ct);
            return entity?.ToResult();
        }

    }
}
