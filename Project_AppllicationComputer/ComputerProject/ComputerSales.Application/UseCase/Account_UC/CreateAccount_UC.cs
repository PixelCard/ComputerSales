using AutoMapper;
using ComputerSales.Application.Interface.Account_Interface;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.Account_DTO;
using ComputerSales.Domain.Entity;
using FluentValidation;

namespace ComputerSales.Application.UseCase.Account_UC
{
    public class CreateAccount_UC
    {

        private readonly IAccountRepository _accountRepo;
        private readonly IUnitOfWorkApplication _uow;
        private readonly IValidator<AccountDTOInput> _validator;
        private readonly IMapper _mapper;


        public CreateAccount_UC(IAccountRepository accountRepo, 
            IUnitOfWorkApplication uow, 
            IValidator<AccountDTOInput> validator,
            IMapper mapper
            )
        {
            _accountRepo = accountRepo;
            _uow = uow;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<AccountOutputDTO2> HandleAsync(AccountDTOInput input, CancellationToken ct = default)
        {
            //Validate 
            await _validator.ValidateAndThrowAsync(input, ct);

            // Map sang entity
            Account entity = _mapper.Map<Account>(input);
                

            // Tạo
            await _accountRepo.AddAccount(entity, ct);

            // Lưu
            await _uow.SaveChangesAsync(ct);

            var created = await _accountRepo.GetAccount(entity.IDAccount, ct);

            var result = _mapper.Map<AccountOutputDTO2>(created);

            return result;
        }
    }
}
