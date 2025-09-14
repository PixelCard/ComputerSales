using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCaseDTO.ProductNews_DTO.UpdateProductsNew
{
    public sealed record InputUpdateProductNews
        (
        int IDProductNews,
        string BlockType,
        string TextContent,
        DateTime CreateDate,
        int DisplayOrder,
        string ImageUrl,
        string Caption
        );
}
