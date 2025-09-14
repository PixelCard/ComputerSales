using AutoMapper;
using ComputerSales.Application.UseCaseDTO.Account_DTO;
using ComputerSales.Application.UseCaseDTO.Customer_DTO;
using ComputerSales.Application.UseCaseDTO.OptionalType_DTO;
using ComputerSales.Domain.Entity;
using ComputerSales.Domain.Entity.ECustomer;
using ComputerSales.Domain.Entity.EOptional;

namespace ComputerSales.Application.Common.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            //Account
            CreateMap<Account, AccountOutputDTO2>();


            CreateMap<AccountDTOInput, Account>();


            //Customer

            CreateMap<CustomerInputDTO, Customer>(); 


            CreateMap<Customer,CustomerOutputDTO>();
            CreateMap<ProductNewsInputDTO, ProductNews>();

        }
    }
}
