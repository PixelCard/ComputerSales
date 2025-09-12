using ComputerSales.Application.UseCaseDTO.Account_DTO.UpdateAccount;
using ComputerSales.Application.UseCaseDTO.Account_DTO;
using ComputerSales.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputerSales.Domain.Entity.ECustomer;

namespace ComputerSales.Application.UseCaseDTO.Customer_DTO
{
    public static class Customer_Mapping
    {
        public static Customer ToEntity(this CustomerInputDTO input)
        {
            return Customer.create(
                input.IMG,
                input.Name,
                input.Description,
                input.Date
            );
        }

        public static CustomerOutputDTO ToResult(this Customer e)
        {
            return new CustomerOutputDTO(
                e.CustomerID,
                e.IMG,
                e.Name,
                e.Description,
                e.Date
            );
        }
    }

}
