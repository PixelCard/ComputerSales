    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCaseDTO.ProductVariant_DTO
{
    public sealed record  VariantPriceRequest(decimal UnitPrice, decimal FinalPrice);
}
