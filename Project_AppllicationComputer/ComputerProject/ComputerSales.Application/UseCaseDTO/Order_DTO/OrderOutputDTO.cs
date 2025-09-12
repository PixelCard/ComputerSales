using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCaseDTO.Order_DTO
{
    public sealed record OrderOutputDTO
    (
        int OrderID,
        DateTime OrderTime,
        int IDCustomer,
        int? PaymentID,
        int OrderStatus,
        decimal GrandTotal,
        bool Status
        );
}
