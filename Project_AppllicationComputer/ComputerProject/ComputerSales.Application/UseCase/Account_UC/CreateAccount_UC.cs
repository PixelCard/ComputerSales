using ComputerSales.Application.Interface.Account_Interface;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.Account_DTO;
using ComputerSales.Domain.Entity;
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

            public CreateAccount_UC(IAccountRepository accountRepo, IUnitOfWorkApplication uow)
            {
                _accountRepo = accountRepo;
                _uow = uow;
            }

            public async Task<AccountOutputDTO> HandleAsync(AccountDTOInput input, CancellationToken ct = default)
            {
                // Validate cơ bản
                if (string.IsNullOrWhiteSpace(input.Email))
                    throw new ArgumentException("Email không được để trống.");
                if (string.IsNullOrWhiteSpace(input.Pass))
                    throw new ArgumentException("Mật khẩu không được để trống.");

                //  kiểm tra trùng email
                var existed = await _accountRepo.GetAccountByEmail(input.Email, ct);
                if (existed is not null)
                    throw new InvalidOperationException("Email đã được sử dụng, vui lòng chọn email khác.");

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
