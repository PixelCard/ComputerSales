using ComputerSales.Application.Interface.Account_Interface;
using ComputerSales.Application.UseCaseDTO.Account_DTO.GetAccountByID;
using ComputerSales.Application.UseCaseDTO.Account_DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputerSales.Application.UseCaseDTO.Account_DTO.GetAccountByEmail;

namespace ComputerSales.Application.UseCase.Account_UC
{
    public class GetAccountByEmail_UC
    {
        private readonly IAccountRepository _repo;
        public GetAccountByEmail_UC(IAccountRepository repo) => _repo = repo;

        public async Task<AccountOutputDTO?> HandleAsync(getAccountByEmailInput input, CancellationToken ct = default)
        {
            var entity = await _repo.GetAccountByEmail(input.Email, ct);

            if (entity is null) return null;

            return entity.ToResult(); 
        }
    }
}
