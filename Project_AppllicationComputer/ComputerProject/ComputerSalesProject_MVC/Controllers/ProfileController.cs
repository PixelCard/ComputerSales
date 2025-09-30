using ComputerSales.Application.UseCase.Account_UC;
using ComputerSales.Application.UseCase.Customer_UC;
using ComputerSales.Application.UseCaseDTO.Account_DTO.GetAccountByID;
using ComputerSalesProject_MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Domain.Entity.E_Order;
using ComputerSales.Application.UseCaseDTO.Customer_DTO.getCustomerByUserID;

namespace ComputerSalesProject_MVC.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ProfileController : Controller
    {
        private readonly GetAccount_UC _getAccount;
        private readonly getCustomerByUserID _getCustomerByUserID;
        private readonly IRespository<Order> _orderRepo;
        private readonly IRespository<OrderDetail> _orderDetailRepo;

        public ProfileController(GetAccount_UC getAccount, getCustomerByUserID getCustomerByUserID, IRespository<Order> orderRepo, IRespository<OrderDetail> orderDetailRepo)
        {
            _getAccount = getAccount;
            _getCustomerByUserID = getCustomerByUserID;
            _orderRepo = orderRepo;
            _orderDetailRepo = orderDetailRepo;
        }

        [HttpGet("AccountInfo")]
        public async Task<IActionResult> AccountInfo(CancellationToken ct)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId) || userId <= 0)
                return RedirectToAction("Login", "Account");

            var account = await _getAccount.HandleAsync(new getAccountByID(userId), ct);
            if (account == null)
                return RedirectToAction("Login", "Account");

            var vm = new AccountInfoViewModel
            {
                AccountId = account.IDAccount,
                Email = account.Email,
                MaskedPassword = "********"
            };

            return View(vm);
        }

        [HttpGet("CustomerInfo")] 
        public async Task<IActionResult> CustomerInfo(CancellationToken ct)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId) || userId <= 0)
                return RedirectToAction("Login", "Account");

            var c = await _getCustomerByUserID.HandleAsync(
                new CustomerGetCustomerByUserID_Request(userId), ct);

            var vm = new CustomerDetailsViewModel
            {
                AccountId = userId,
                FullName = c?.Name ?? string.Empty,
                Email = string.Empty, // không lưu email trong customer, có thể lấy từ Account nếu cần
                Phone = c?.sdt ?? string.Empty,
                Address = c?.address ?? string.Empty,
                BirthDate = c?.Date
            };

            return View(vm);
        }

        [HttpGet("OrdersInfo")] 
        public async Task<IActionResult> OrdersInfo(CancellationToken ct)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId) || userId <= 0)
                return RedirectToAction("Login", "Account");

            var customer = await _getCustomerByUserID.HandleAsync(
                new CustomerGetCustomerByUserID_Request(userId), ct);
            if (customer == null)
            {
                return View(new OrdersListViewModel());
            }

            var orders = await _orderRepo.ListAsync(o => o.IDCustomer == customer.IDCustomer, orderBy: q => q.OrderByDescending(x => x.OrderTime), ct: ct);

            var orderIds = orders.Select(o => o.OrderID).ToArray();

            var details = await _orderDetailRepo.ListAsync(d => orderIds.Contains(d.OrderID));

            var grouped = details.GroupBy(d => d.OrderID).ToDictionary(g => g.Key, g => g.ToList());


            var vm = new OrdersListViewModel
            {
                Orders = orders.Select(o => new OrderWithDetailsViewModel
                {
                    OrderID = o.OrderID,
                    OrderTime = o.OrderTime,
                    Status = o.OrderStatus.ToString(),
                    GrandTotal = o.GrandTotal,
                    Details = (grouped.TryGetValue(o.OrderID, out var list)
                                ? list
                                : new List<OrderDetail>())
                               .Select(d => new OrderDetailItemViewModel
                               {
                                   ProductName = d.Name,
                                   Quantity = d.Quantity,
                                   UnitPrice = d.UnitPrice,
                                   Discount = d.Discount,
                                   ImageUrl = d.ImageUrl,
                                   OptionSummary = d.OptionSummary
                               }).ToList()
                }).ToList()
            };

            return View(vm);
        }
    }
}


