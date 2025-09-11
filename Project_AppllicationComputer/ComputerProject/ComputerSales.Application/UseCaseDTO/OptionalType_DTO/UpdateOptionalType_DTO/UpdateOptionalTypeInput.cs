using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCaseDTO.OptionalType_DTO.UpdateOptionalType_DTO
{
    public sealed record UpdateOptionalTypeInput(
        int id,
        string Code,
        string Name);
}
