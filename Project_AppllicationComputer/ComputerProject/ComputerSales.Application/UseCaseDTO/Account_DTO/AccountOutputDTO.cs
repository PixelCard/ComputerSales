using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCaseDTO.Account_DTO
{
    public sealed record AccountOutputDTO(int IDAccount,  string Email, int IDRole,string TenRole);
}
