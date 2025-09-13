using ComputerSales.Application.Interface.Account_Interface;
using ComputerSales.Application.UseCaseDTO.Account_DTO;
using FluentValidation;

namespace ComputerSales.Application.Validator.AccountValidator
{
    public class CreateAccountValidator : AbstractValidator<AccountDTOInput>
    {
        public CreateAccountValidator(IAccountRepository accountRepo)
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email không được để trống.")
                .EmailAddress().WithMessage("Email không hợp lệ.")
                .MustAsync(async (email, ct) =>
                {
                    var existed = await accountRepo.GetAccountByEmail(email, ct);
                    return existed is null;
                }).WithMessage("Email đã được sử dụng, vui lòng chọn email khác.");

            RuleFor(x => x.Pass)
                .NotEmpty().WithMessage("Mật khẩu không được để trống.")
                .MinimumLength(6).WithMessage("Mật khẩu tối thiểu 6 ký tự.");
        }
    }
}
