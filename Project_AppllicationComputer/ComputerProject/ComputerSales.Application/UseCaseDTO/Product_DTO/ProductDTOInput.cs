using ComputerSales.Domain.Entity.EProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCaseDTO.Product_DTO
{
    public sealed record ProductDTOInput(
        string ShortDescription,
        int Status,
        long AccessoriesID,
        long ProviderID,
        string Slug,
        string SKU
    );
}
