using ComputerSales.Application.Payment.Interface;
using ComputerSales.Application.Payment.VNPAY.Entity;
using ComputerSales.Application.Payment.VNPAY.Libary;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace ComputerSales.Application.Payment.VNPAY.Respository
{
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _configuration;

        public VnPayService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreatePaymentUrl(PaymentInformation model, HttpContext context)
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
            var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tz);

            var pay = new VnPayLibrary();
            var urlBack = _configuration["Vnpay:PaymentBackReturnUrl"];
            var amount100 = (long)Math.Round(model.Amount * 100m);

            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", amount100.ToString());
            pay.AddRequestData("vnp_CreateDate", now.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", model.OrderDescription ?? "");
            pay.AddRequestData("vnp_OrderType", model.OrderType ?? "other");
            pay.AddRequestData("vnp_IpAddr", "127.0.0.1");
            pay.AddRequestData("vnp_ReturnUrl", urlBack);

            // dùng session.Id làm TxnRef
            pay.AddRequestData("vnp_TxnRef", model.TxnRef);

            var paymentUrl =
                pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);

            return paymentUrl;

        }

        public PaymentVNPAY_Response PaymentExecute(IQueryCollection collections)
        {
            var pay = new VnPayLibrary();
            var response = pay.GetFullResponseData(collections, _configuration["Vnpay:HashSecret"]);

            return response;

        }
    }
}
