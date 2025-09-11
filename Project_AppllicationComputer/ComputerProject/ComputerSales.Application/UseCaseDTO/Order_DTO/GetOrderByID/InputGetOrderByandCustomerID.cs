using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCaseDTO.Order_DTO.GetOrderByID
{
    public sealed record InputGetOrderByandCustomerID(int IDCustomer, int OrderID);
}
