using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCaseDTO.VariantPrice_DTO
{
    public sealed record variantGetPriceByVariantID_Output(decimal unitPrice,decimal DiscountPrice);
}
