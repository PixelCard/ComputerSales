using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCaseDTO.Order_DTO.UpdateOrder
{
    public sealed record InputUpdateOrderByIdOrderCustomer(
      int OrderID,
      int CustomerID,
      int OrderStatus,
      decimal GrandTotal,
      bool Status
      );
}
