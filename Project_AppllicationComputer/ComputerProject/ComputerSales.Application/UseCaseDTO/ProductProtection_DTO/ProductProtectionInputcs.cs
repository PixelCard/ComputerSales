using ComputerSales.Domain.Entity.EProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCaseDTO.ProductProtection_DTO
{
    public sealed record ProductProtectionInputcs(
        DateTime DateBuy,
        DateTime DateEnd,
        WarrantyStatus Status,
        long ProductId);
}
