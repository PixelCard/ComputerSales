using ComputerSales.Domain.Entity.EProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCaseDTO.ProductOptionalType_DTO
{
    public static class ProductOptionalTypeMapping
    {
        public static ProductOptionType ToEnity(this ProducyOptionalTypeInput input)
        {
            return ProductOptionType.create(
                input.ProductId,
                input.OptionTypeId
            );
        }


        public static ProductOptionalTypeOutput ToResult(this ProductOptionType input)
        {
            return new ProductOptionalTypeOutput(
                input.ProductId, 
                input.OptionTypeId
            );
        }
    }
}
