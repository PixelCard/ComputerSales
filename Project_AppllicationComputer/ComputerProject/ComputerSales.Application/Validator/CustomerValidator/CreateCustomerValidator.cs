using ComputerSales.Application.UseCaseDTO.Customer_DTO;
using ComputerSales.Domain.Entity.ECustomer;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.Validator.CustomerValidator
{
    public class CreateCustomerValidator : AbstractValidator<CustomerInputDTO>
    {
        public CreateCustomerValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Description).MaximumLength(500);
            RuleFor(x => x.IMG).MaximumLength(255).When(x => !string.IsNullOrWhiteSpace(x.IMG));
            RuleFor(x => x.Date).LessThanOrEqualTo(DateTime.Today);
            RuleFor(x => x.IDAccount).GreaterThan(0).WithMessage("IDAccount không hợp lệ");
        }
    }
}
