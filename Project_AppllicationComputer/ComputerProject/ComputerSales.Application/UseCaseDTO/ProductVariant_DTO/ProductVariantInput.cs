using ComputerSales.Domain.Entity.EProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCaseDTO.ProductVariant_DTO
{
    public sealed record ProductVariantInput(
     long ProductId,
     string SKU ,
     VariantStatus Status ,
     int Quantity );
}
