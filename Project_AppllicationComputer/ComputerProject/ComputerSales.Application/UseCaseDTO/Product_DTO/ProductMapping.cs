using ComputerSales.Domain.Entity.EProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCaseDTO.Product_DTO
{
    public static class ProductMapping
    {
        public static Product ToEnity(this ProductDTOInput input)
        {
            return Product.Create(
                input.AccessoriesID,
                input.ProviderID,
                input.ShortDescription,
                input.SKU,
                input.Slug
            );
        }


        public static ProductOutputDTOcs ToResult(this Product output)
        {
            return new ProductOutputDTOcs(output.ProductID,output.Slug,output.AccessoriesID,output.ProviderID);
        }
    }
}
