using ComputerSales.Application.UseCaseDTO.OptionalType_DTO;
using ComputerSales.Domain.Entity.EOptional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCaseDTO.OptionalValue_DTO
{
    public static class OptionalValueMapping
    {
        public static OptionalValue ToEnity(this OptionalValueInput input)
            => OptionalValue.create(input.OptionTypeId,input.Value,input.SortOrder);

        public static OptionalValueOutput ToResult(this OptionalValue input)
            => new OptionalValueOutput(input.Id,input.OptionTypeId,input.Value,input.SortOrder);
    }
}
