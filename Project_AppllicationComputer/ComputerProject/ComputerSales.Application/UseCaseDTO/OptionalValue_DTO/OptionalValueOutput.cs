using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCaseDTO.OptionalValue_DTO
{
    public sealed record OptionalValueOutput(
         int Id,
         int OptionTypeId,
         string Value,
         int SortOrder );
}
