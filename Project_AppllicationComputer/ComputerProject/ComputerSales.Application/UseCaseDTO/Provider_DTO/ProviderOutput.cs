using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCaseDTO.Provider_DTO
{
    public sealed record ProviderOutput(
        long ProviderID,
        string ProviderName);
}
