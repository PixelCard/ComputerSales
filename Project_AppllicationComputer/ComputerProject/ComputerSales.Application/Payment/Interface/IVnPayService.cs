using ComputerSales.Application.Payment.VNPAY.Entity;
using Microsoft.AspNetCore.Http;

namespace ComputerSales.Application.Payment.Interface
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(PaymentInformation model, HttpContext context);
        PaymentVNPAY_Response PaymentExecute(IQueryCollection collections);
    }
}
