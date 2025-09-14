using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCaseDTO.OrderDetails_DTO.DeleteOrderDetails
{
    public sealed record InputDeleteOrderDetails
    (
        int OrderID,
      long? ProductID,
      int ProductVariantID
    );
}
