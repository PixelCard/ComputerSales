using AutoMapper;
using ComputerSales.Application.UseCaseDTO.Account_DTO;
using ComputerSales.Domain.Entity;

namespace ComputerSales.Application.Common.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Account, AccountOutputDTO2>();
            CreateMap<AccountDTOInput, Account>();
            
        }
    }
}
