using ComputerSales.Application.Interface.Role_Interface;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.Role_DTO.UpdateRole;
using ComputerSales.Application.UseCaseDTO.Role_DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputerSales.Application.Interface.Account_Interface;
using ComputerSales.Application.UseCaseDTO.Account_DTO;
using ComputerSales.Application.UseCaseDTO.Account_DTO.UpdateAccount;

namespace ComputerSales.Application.UseCase.Account_UC
{
    public class UpdateAccount_UC
    {
        private readonly IAccountRepository _repo;
        private readonly IUnitOfWorkApplication _uow;

        public UpdateAccount_UC(IAccountRepository repo, IUnitOfWorkApplication uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<AccountOutputDTO?> HandleAsync(UpdateAccountDTO input, CancellationToken ct = default)
        {
            var entity = await _repo.GetAccountByID(input.IDRole, ct);
            if (entity is null) return null;

            // nếu dùng concurrency
            // if (!string.IsNullOrWhiteSpace(input.RowVersionBase64))
            //     entity.RowVersion = Convert.FromBase64String(input.RowVersionBase64);

            entity.Email = input.Email.Trim();  
            

            await _repo.UpdateAccount(entity, ct);
            await _uow.SaveChangesAsync(ct);

            return entity.ToResult();
        }

    }

}
