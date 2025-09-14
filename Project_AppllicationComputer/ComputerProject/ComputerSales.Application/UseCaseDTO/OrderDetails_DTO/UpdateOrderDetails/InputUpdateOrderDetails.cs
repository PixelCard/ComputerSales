using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCaseDTO.OrderDetails_DTO.UpdateOrderDetails
{
    public sealed record InputUpdateOrderDetails(
        int Quantity,
        decimal UnitPrice,
        decimal Discount
    );


}
