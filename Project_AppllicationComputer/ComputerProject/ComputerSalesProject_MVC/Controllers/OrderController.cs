using ComputerSales.Application.Interface.Interface_OrderFromCart;
using ComputerSales.Application.Payment.Interface;
using ComputerSales.Application.UseCase.Account_UC;
using ComputerSales.Application.UseCase.Cart_UC.Queries.GetCartPage;
using ComputerSales.Application.UseCase.Customer_UC;
using ComputerSales.Application.UseCaseDTO.Account_DTO.GetAccountByID;
using ComputerSales.Application.UseCaseDTO.Customer_DTO.getCustomerByUserID;
using ComputerSales.Application.UseCaseDTO.Order_DTO;
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

        private readonly IOrderFromCart _addOrder;

        private readonly GetCartPageQueryHandler getCartPageQueryHandler;

        public OrderController(
            getCustomerByUserID getCustomerByUserID, 
            GetCartPageQueryHandler getCartPageQueryHandler, 
            GetAccount_UC getAccount_UC,
            IOrderFromCart _addOrder,
            IVnPayService _vnPayService)
        {
            this.getCustomerByUserID = getCustomerByUserID;
            this.getCartPageQueryHandler = getCartPageQueryHandler;
            this.getAccount_UC= getAccount_UC;
            this._addOrder = _addOrder;
            this._vnPayService= _vnPayService;
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

            //Tạo đơn hàng từ giỏ hàng hiện tại

            var orderId = await _addOrder.CreateFromCartAsync(userID, input.FullName, input.Phone,
                               input.Email, input.Address, input.Notes, input.Payment, ct);


            return RedirectToAction(nameof(Success), new { id = orderId });
        }

        [HttpGet]
        public IActionResult Success(int id) => View(model: id);


        [HttpGet]
        public IActionResult PaymentCallbackVnpay()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            return Json(response);
        }
    }
}
