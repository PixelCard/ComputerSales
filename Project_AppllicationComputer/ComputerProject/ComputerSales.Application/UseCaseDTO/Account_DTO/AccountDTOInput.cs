using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCaseDTO.Account_DTO
{
    public sealed record AccountDTOInput( string Email, string Pass, int IDRole );
}
