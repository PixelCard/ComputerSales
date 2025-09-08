using ComputerSales.Domain.Entity.EProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCaseDTO.ProductOverView_DTO.UpdateDTO
{
    public sealed record UpdateInputDTO(int ProductOverViewID, string TextContent,string ImgURL,string Caption);
}
