using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCaseDTO.OrderDetails_DTO.getOrderDetailsByID
{
    public sealed record InputGetOrderDetailsById
    (
      int OrderID ,
      long? ProductID ,
      int ProductVariantID );
}
