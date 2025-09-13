using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCaseDTO.ProductNews_DTO
{
    public sealed record ProductNewsInputDTO
    (
      string BlockType,
      string? TextContent ,
      string? ImageUrl ,
      string? Caption ,
      int DisplayOrder ,
      DateTime CreateDate
    );
  
}
