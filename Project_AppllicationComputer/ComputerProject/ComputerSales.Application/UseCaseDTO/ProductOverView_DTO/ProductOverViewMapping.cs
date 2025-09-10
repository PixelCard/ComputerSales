using ComputerSales.Domain.Entity.EProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCaseDTO.ProductOverView_DTO
{
    public static class ProductOverViewMapping
    {
        public static ProductOverview ToEnity(this ProductOverViewInput input)
        {
            return ProductOverview.Create(
                input.ProductId,
                input.BlockType,
                input.TextContent,
                input.ImageUrl,
                input.Caption,
                input.DisplayOrder
            );
        }

        public static ProductOverViewOutput ToResult(this ProductOverview e)
        {
            return new ProductOverViewOutput(
                e.ProductOverviewId,
                e.ProductId,
                e.BlockType,
                e.TextContent,
                e.ImageUrl,
                e.Caption,
                e.DisplayOrder
            );
        }
    }
}
