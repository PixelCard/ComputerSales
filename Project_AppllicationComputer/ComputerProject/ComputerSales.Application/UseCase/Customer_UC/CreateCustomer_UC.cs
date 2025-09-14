using AutoMapper;
using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.Account_DTO;
using ComputerSales.Application.UseCaseDTO.Customer_DTO;
using ComputerSales.Application.UseCaseDTO.OptionalType_DTO;
using ComputerSales.Domain.Entity.ECustomer;
using FluentValidation;

namespace ComputerSales.Application.UseCase.Customer_UC
{
    public class CreateCustomer_UC
    {
        private IRespository<Customer> respository;

        private IUnitOfWorkApplication unitOfWorkApplication;

        private readonly IValidator<CustomerInputDTO> _validator;

        private readonly IMapper mapper;

        public CreateCustomer_UC(IRespository<Customer> respository,
            IUnitOfWorkApplication unitOfWorkApplication,
            IValidator<CustomerInputDTO> validator,
            IMapper mapper)
        {
            this.respository = respository;
            this.unitOfWorkApplication = unitOfWorkApplication;
            this._validator = validator;
            this.mapper = mapper;
        }

        public async Task<CustomerOutputDTO?> HandleAsync(CustomerInputDTO input, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(input, ct);

            Customer entity = mapper.Map<Customer>(input); 

            await respository.AddAsync(entity, ct);

            await unitOfWorkApplication.SaveChangesAsync(ct);

            var result = mapper.Map<CustomerOutputDTO>(entity);

            return result;
        }
    }
}
