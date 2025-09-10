using ComputerSales.Application.UseCaseDTO.ProductProtection_DTO;
using ComputerSales.Domain.Entity.EProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCaseDTO.ProductVariant_DTO
{
    public static class ProductVariantMapping
    {
        public static ProductVariant ToEnity(this ProductVariantInput input)
        {
            return ProductVariant.create(
                input.ProductId,
                input.SKU,
                input.Status,
                input.Quantity
            );
        }

        public static ProductVariantOutput ToResult(this ProductVariant e)
        {
            return new ProductVariantOutput(
                e.SKU, 
                e.Status, 
                e.Quantity,
                e.Id
            );
        }
    }
}
