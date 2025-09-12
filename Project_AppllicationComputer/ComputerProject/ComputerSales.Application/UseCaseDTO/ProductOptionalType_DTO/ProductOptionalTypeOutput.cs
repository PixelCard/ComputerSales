using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCaseDTO.ProductOptionalType_DTO
{
    public sealed record ProductOptionalTypeOutput(
        long ProductId,
        int OptionTypeId);
}
