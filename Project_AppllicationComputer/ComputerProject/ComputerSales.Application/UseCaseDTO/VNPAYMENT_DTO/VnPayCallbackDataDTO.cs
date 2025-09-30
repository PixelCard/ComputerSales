using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCaseDTO.VNPAYMENT_DTO
{
    public class VnPayCallbackDataDTO
    {
        public string TransactionId { get; set; } = "";
        public string ResponseCode { get; set; } = "";
        public decimal Amount { get; set; }
    }
}
