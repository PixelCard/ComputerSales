using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCaseDTO.Product_DTO
{
    public sealed record ProductOutputDTOcs(
         long ProductID,

         string Slug,

         long AccessoriesID,

         long ProviderID
    );
}
