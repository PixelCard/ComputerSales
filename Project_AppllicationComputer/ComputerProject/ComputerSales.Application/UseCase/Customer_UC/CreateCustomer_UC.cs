using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.Account_DTO;
using ComputerSales.Application.UseCaseDTO.Customer_DTO;
using ComputerSales.Application.UseCaseDTO.OptionalType_DTO;
using ComputerSales.Domain.Entity.ECustomer;
using ComputerSales.Domain.Entity.EOptional;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCase.Customer_UC
{
    public class CreateCustomer_UC
    {
        private IRespository<Customer> respository;

        private IUnitOfWorkApplication unitOfWorkApplication;

        private readonly IValidator<CustomerInputDTO> _validator;

        public CreateCustomer_UC(IRespository<Customer> respository,
            IUnitOfWorkApplication unitOfWorkApplication,
            IValidator<CustomerInputDTO> validator)
        {
            this.respository = respository;
            this.unitOfWorkApplication = unitOfWorkApplication;
            this._validator = validator;
        }

        public async Task<CustomerOutputDTO?> HandleAsync(CustomerInputDTO input, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(input, ct);

            Customer entity = input.ToEntity();

            await respository.AddAsync(entity, ct);

            await unitOfWorkApplication.SaveChangesAsync(ct);

            return entity.ToResult();
        }
    }
}
