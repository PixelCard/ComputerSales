using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCaseDTO.Account_DTO.ForgetPasswordDTO
{
    public class ForgotResetDto
    {
        public string Email { get; set; } = ""; 
        public string Token { get; set; } = ""; 
        public string NewPassword { get; set; } = "";
    }
}
