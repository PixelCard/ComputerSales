using ComputerSales.Application.UseCaseDTO.Customer_DTO;
using FluentValidation;
using System;

namespace ComputerSales.Application.Validator.CustomerValidator
{
    public class CreateCustomerValidator : AbstractValidator<CustomerInputDTO>
    {
        public CreateCustomerValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Không được để trống thộng tin tên của bạn").MaximumLength(100);
            RuleFor(x => x.Description).MaximumLength(500);
            RuleFor(x => x.IMG).MaximumLength(255).When(x => !string.IsNullOrWhiteSpace(x.IMG));
            RuleFor(x => x.Date).LessThanOrEqualTo(DateTime.Today);
            RuleFor(x => x.address).NotEmpty().WithMessage("Không được để trống thông tin về Địa chỉ").MaximumLength(40).WithMessage("Độ dài kí tự không vượt quá 40 kí tự");
            RuleFor(x => x.sdt).MaximumLength(12).WithMessage("Số điện thoại không được vượt quá 12 số");
            RuleFor(x => x.IDAccount).GreaterThan(0).WithMessage("IDAccount không hợp lệ");
        }
    }
}
