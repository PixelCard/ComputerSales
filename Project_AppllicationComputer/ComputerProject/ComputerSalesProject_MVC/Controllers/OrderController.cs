using ComputerSales.Application.Interface.Interface_OrderFromCart;
using ComputerSales.Application.Interface.InterfaceVNPAYMENT;
using ComputerSales.Application.Payment.Interface;
using ComputerSales.Application.Payment.VNPAY.Entity;
using ComputerSales.Application.UseCase.Account_UC;
using ComputerSales.Application.UseCase.Cart_UC.Queries.GetCartPage;
using ComputerSales.Application.UseCase.Customer_UC;
using ComputerSales.Application.UseCase.Order_UC;
using ComputerSales.Application.UseCaseDTO.Account_DTO.GetAccountByID;
using ComputerSales.Application.UseCaseDTO.Customer_DTO.getCustomerByUserID;
using ComputerSales.Application.UseCaseDTO.Order_DTO;
using ComputerSales.Application.UseCaseDTO.Order_DTO.GetOrderByID;
using ComputerSales.Application.UseCaseDTO.VNPAYMENT_DTO;
using ComputerSalesProject_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ComputerSalesProject_MVC.Controllers
{
    public class OrderController : Controller
   {
            private readonly getCustomerByUserID getCustomerByUserID;

            private readonly GetAccount_UC getAccount_UC;

            private readonly IVnPayService _vnPayService;

            private readonly GetOrderByID_UC getOrderByID_UC;

            private readonly IOrderFromCart _addOrder;

            private readonly IVnPaySessionService _vnPaySession;

            private readonly GetCartPageQueryHandler getCartPageQueryHandler;

            public OrderController(
                getCustomerByUserID getCustomerByUserID, 
                GetCartPageQueryHandler getCartPageQueryHandler, 
                GetAccount_UC getAccount_UC,
                IOrderFromCart _addOrder,
                IVnPayService _vnPayService,
                IVnPaySessionService _vnPaySession,
                GetOrderByID_UC getOrderByID_UC)
            {
                this.getCustomerByUserID = getCustomerByUserID;
                this.getCartPageQueryHandler = getCartPageQueryHandler;
                this.getAccount_UC= getAccount_UC;
                this._addOrder = _addOrder;
                this._vnPayService= _vnPayService;
                this._vnPaySession = _vnPaySession;
            this.getOrderByID_UC = getOrderByID_UC;
            }


            public async Task<IActionResult> OrderHome(CancellationToken ct)
            {
                int userID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value ?? "0");

                if(userID == 0)
                {
                    return RedirectToAction("Login","Account");   
                }

                var request = new CustomerGetCustomerByUserID_Request(userID);

                var cartVm = await getCartPageQueryHandler.Handle(new GetCartPageQuery(userID), ct);

                var customer = await getCustomerByUserID.HandleAsync(new CustomerGetCustomerByUserID_Request(userID), ct);

                var requestAccount = new getAccountByID(userID);

                var accountDTO = getAccount_UC.HandleAsync(requestAccount, ct);

                var customerVM = CustomerSummaryViewModel.createCustomerSummaryViewModel(
                    userID,
                    customer.Name,
                    accountDTO.Result.Email,
                    customer.sdt,
                    customer.address
                );

                var vm = new OrderPageViewModel
                {
                    cartPageDTO = cartVm,
                    CustomerSummary = customerVM,
                    orderCheckoutInput = new OrderCheckoutInput
                    {
                        FullName = customerVM.FullName,
                        Phone = customerVM.Phone,
                        Email = string.IsNullOrWhiteSpace(customerVM.Email) ? null : customerVM.Email,
                        Address = customerVM.Address,
                        Payment = PaymentKind.COD
                    }
                       
                };

                return View(vm);
            }



            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> PlaceOrder([Bind(Prefix = "orderCheckoutInput")] OrderCheckoutInput input, CancellationToken ct)
            {
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!int.TryParse(userIdStr, out var userID) || userID <= 0)
                    return RedirectToAction("Login", "Account");

                if (!ModelState.IsValid)
                {
                    // Re-load lại page với lỗi
                    var cartVm = await getCartPageQueryHandler.Handle(new GetCartPageQuery(userID), ct);

                    var request = new CustomerGetCustomerByUserID_Request(userID);

                    var customerDTO = await getCustomerByUserID.HandleAsync(request, ct);

                    if(customerDTO == null)
                    {
                        var customerVM2 = new CustomerSummaryViewModel { AccountId = userID };
                    }

                    var requestAccount = new getAccountByID(userID);

                    var accountDTO = getAccount_UC.HandleAsync(requestAccount, ct);

                    var customerVM = CustomerSummaryViewModel.createCustomerSummaryViewModel(
                        userID,
                        customerDTO.Name,
                        accountDTO.Result.Email,
                        customerDTO.sdt,
                        customerDTO.address
                    );

                    return View("OrderHome", new OrderPageViewModel
                    {
                        cartPageDTO = cartVm,
                        CustomerSummary = customerVM,
                        orderCheckoutInput = input
                    });
                }

                if (input.Payment == PaymentKind.VNPAY)
                {
                    // 1) Tính tổng giỏ 
                    var cartVm = await getCartPageQueryHandler.Handle(new GetCartPageQuery(userID), ct);

                    var amount = cartVm?.GrandTotal ?? 0m;

                    // 2) Tạo session qua service
                    var session = await _vnPaySession.CreatePendingAsync(userID, amount, ct);

                    // 3) Tạo URL & redirect
                    var url = _vnPayService.CreatePaymentUrl(new PaymentInformation
                    {
                        TxnRef = session.TxnRef, // vnp_TxnRef
                        Amount = amount,
                        Name = "Thanh toán giỏ hàng",
                        OrderType = "other",
                        OrderDescription = $"uid={userID}"
                    }, HttpContext);

                    return Redirect(url);
                }

                // COD: tạo ngay
                var orderId = await _addOrder.CreateFromCartAsync(
                    userID, input.FullName, input.Phone, input.Email, input.Address, input.Notes, input.Payment, ct);
                return RedirectToAction(nameof(Success), new { id = orderId });
            }




            [HttpGet("Success/{id:long}")]
            public async Task<IActionResult> Success(int id,CancellationToken ct)
            {
                    var order = await getOrderByID_UC.HandleAsync(new InputGetOrderByID(id),ct);
                    return View(order);
            }



            [HttpGet]
            public IActionResult Failed(PaymentVNPAY_Response response) => View(model: response);




            [HttpGet]
            public async Task<IActionResult> PaymentCallbackVnpay(CancellationToken ct)
            {
                var resp = _vnPayService.PaymentExecute(Request.Query);
                var ok = resp.VnPayResponseCode == "0" || resp.VnPayResponseCode == "00";
                if (!ok) return View("Failed", resp);

                var txnRef = resp.OrderId; // "000000000123" (chuỗi số)
                var session = await _vnPaySession.GetByTxnRefAsync(txnRef, ct);
                if (session == null) return View("Failed", "SESSION_NOT_FOUND");

                if (session.Status == "Completed" && session.OrderId.HasValue)
                    return RedirectToAction(nameof(Success), new { id = session.OrderId.Value });

                // tạo Order
                var customer = await getCustomerByUserID.HandleAsync(new CustomerGetCustomerByUserID_Request(session.UserId), ct);
                var account = await getAccount_UC.HandleAsync(new getAccountByID(session.UserId), ct);

                var newOrderId = await _addOrder.CreateFromCartAsync(
                    session.UserId,
                    customer?.Name ?? "Khách hàng",
                    customer?.sdt ?? "",
                    account?.Email,
                    customer?.address ?? "",
                    null,
                    PaymentKind.VNPAY,
                    ct
                );

                await _addOrder.MarkPaidAsync(newOrderId, PaymentKind.VNPAY, resp.TransactionId, resp.VnPayResponseCode, ct);

                await _vnPaySession.CompleteAsync(session.Id, newOrderId, new VnPayCallbackDataDTO
                {
                    TransactionId = resp.TransactionId,
                    ResponseCode = resp.VnPayResponseCode,
                    Amount = session.Amount // hoặc bóc vnp_Amount từ query để log
                }, ct);

                // Bạn có thể render TxnRef (số) ra GUI nếu muốn
                TempData["TxnRef"] = txnRef;

                return RedirectToAction(nameof(Success), new { id = newOrderId });
            }
    }
}


