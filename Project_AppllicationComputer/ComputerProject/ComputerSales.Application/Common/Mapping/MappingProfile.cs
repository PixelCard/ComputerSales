using AutoMapper;
using ComputerSales.Application.UseCaseDTO.Account_DTO;
using ComputerSales.Application.UseCaseDTO.ProductNews_DTO;
using ComputerSales.Domain.Entity;
using ComputerSales.Domain.Entity.ENews;

namespace ComputerSales.Application.Common.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AccountDTOInput, Account>();
            CreateMap<ProductNewsInputDTO, ProductNews>();

        }
    }
}
