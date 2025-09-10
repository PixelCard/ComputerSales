using ComputerSales.Application.Interface.Account_Interface;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.Account_DTO;
using ComputerSales.Domain.Entity;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCase.Account_UC
{
    public class CreateAccount_UC
    {

        private readonly IAccountRepository _accountRepo;
        private readonly IUnitOfWorkApplication _uow;
        private readonly IValidator<AccountDTOInput> _validator;


        public CreateAccount_UC(IAccountRepository accountRepo, 
            IUnitOfWorkApplication uow, 
            IValidator<AccountDTOInput> validator)
        {
            _accountRepo = accountRepo;
            _uow = uow;
            _validator = validator;
        }

        public async Task<AccountOutputDTO> HandleAsync(AccountDTOInput input, CancellationToken ct = default)
        {
            //Validate 
            await _validator.ValidateAndThrowAsync(input, ct);

            // Map sang entity
            Account entity = input.ToEntity();

            // Tạo
            await _accountRepo.AddAccount(entity, ct);

            // Lưu
            await _uow.SaveChangesAsync(ct);

            // (tuỳ bạn) nếu muốn có TenRole trong output, cần Include Role khi lấy lại,
            // hoặc đơn giản trả luôn mà chấp nhận TenRole trống lần đầu.
            // Ở đây lấy lại để có Role:
            var created = await _accountRepo.GetAccount(entity.IDAccount, ct);

            return (created ?? entity).ToResult();
        }
    }
}
