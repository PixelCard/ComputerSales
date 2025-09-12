using ComputerSales.Domain.Entity.EOptional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCaseDTO.OptionalType_DTO
{
    public static class OptionalTypeMapping
    {
        public static OptionType ToEnity(this OptionalTypeInput input)
            => OptionType.create(input.Code, input.Name);   

        public static OptionalTypeOutput ToResult(this OptionType input)
            => new OptionalTypeOutput(input.Id,input.Code, input.Name);
    }
}
