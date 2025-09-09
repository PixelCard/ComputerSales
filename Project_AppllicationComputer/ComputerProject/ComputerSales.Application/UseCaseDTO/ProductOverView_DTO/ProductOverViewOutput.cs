using ComputerSales.Domain.Entity.EProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCaseDTO.ProductOverView_DTO
{
    public sealed record ProductOverViewOutput(int ProductOverviewId, long ProductId, OverviewBlockType BlockType, string TextContent,
            string? ImageUrl, string? Caption, int DisplayOrder);
}
