using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCaseDTO.Customer_DTO.UpdateCustomerDTO
{
    public sealed record  InputUpdateCustomerDTO
    (
      string? IMG ,
      string Name ,
      string? Description ,
      DateTime Date 
        
    );
}
