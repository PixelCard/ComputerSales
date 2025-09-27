using ComputerSales.Application.Payment.VNPAY.Entity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.Payment.Interface
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(PaymentInformation model, HttpContext context);
        PaymentVNPAY_Response PaymentExecute(IQueryCollection collections);

    }
}
