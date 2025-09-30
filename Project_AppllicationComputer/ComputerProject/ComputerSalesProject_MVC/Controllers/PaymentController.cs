using ComputerSales.Application.Payment.Interface;
using ComputerSales.Application.Payment.VNPAY.Entity;
using Microsoft.AspNetCore.Mvc;

namespace ComputerSalesProject_MVC.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IVnPayService _vnPayService;
        public PaymentController(IVnPayService vnPayService)
        {

            _vnPayService = vnPayService;
        }

        public IActionResult CreatePaymentUrlVnpay(PaymentInformation model)
        {
            var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

            return Redirect(url);
        }
    }
}
