using ComputerSales.Domain.Entity.ECustomer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.Interface.InterfaceCustomerRespo
{
    public interface ICustomerRespo
    {
        Task<Customer> GetCustomerByUserID(int userID,CancellationToken ct);
    }
}
