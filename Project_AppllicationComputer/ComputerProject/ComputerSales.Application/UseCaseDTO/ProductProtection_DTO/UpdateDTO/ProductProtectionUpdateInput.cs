using ComputerSales.Domain.Entity.EProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCaseDTO.ProductProtection_DTO.UpdateDTO
{
    public sealed record ProductProtectionUpdateInput(
        long ProtectionProductId,
        DateTime DateBuy,
         DateTime DateEnd,
         WarrantyStatus Status);
}
