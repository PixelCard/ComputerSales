using ComputerSales.Domain.Entity.EVariant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCaseDTO.VariantPrice_DTO.UpdateVariantPrice
{
    public sealed record UpdateVariantPriceInput(
     int VariantId,
     string Currency,
     decimal Price,
     decimal DiscountPrice,
     PriceStatus Status,
     DateTime? ValidFrom,
     DateTime? ValidTo
    );

}
