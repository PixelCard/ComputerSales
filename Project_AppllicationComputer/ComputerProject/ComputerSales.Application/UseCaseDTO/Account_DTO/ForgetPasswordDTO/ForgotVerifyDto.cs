using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCaseDTO.Account_DTO.ForgetPasswordDTO
{
    public class ForgotVerifyDto
    {
        public string Email { get; set; } = ""; 
        public string Code { get; set; } = "";
    }
}
