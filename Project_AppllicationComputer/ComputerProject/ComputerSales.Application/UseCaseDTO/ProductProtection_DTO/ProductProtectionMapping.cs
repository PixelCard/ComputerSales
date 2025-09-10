using ComputerSales.Application.UseCaseDTO.ProductOverView_DTO;
using ComputerSales.Domain.Entity.EProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCaseDTO.ProductProtection_DTO
{
    public static class ProductProtectionMapping
    {
        public static ProductProtection ToEnity(this ProductProtectionInputcs input)
        {
            return ProductProtection.create(
                input.ProductId,
                input.DateBuy,
                input.DateEnd,
                input.Status
            );
        }

        public static ProductProtectionOutput ToResult(this ProductProtection e)
        {
            return new ProductProtectionOutput(
               e.ProtectionProductId,
               e.DateBuy,
               e.DateEnd,
               e.Status,
               e.ProductId
            );
        }
    }
}
