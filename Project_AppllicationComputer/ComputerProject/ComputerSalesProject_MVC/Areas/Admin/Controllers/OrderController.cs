using ComputerSales.Application.UseCase.Order_UC;
using ComputerSales.Application.UseCaseDTO.Order_DTO;
using ComputerSales.Application.UseCaseDTO.Order_DTO.CancelOrder;
using ComputerSales.Application.UseCaseDTO.Order_DTO.GetOrderByID;
using ComputerSales.Application.UseCaseDTO.Order_DTO.GetOrdersList;
using ComputerSales.Domain.Entity.E_Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComputerSalesProject_MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class OrderController : Controller
    {
        private readonly GetOrderByID_UC getOrderByOrderID;
        private readonly CancelOrder_UC cancelOrderUC;
        private readonly GetOrdersList_UC getOrdersListUC;

        public OrderController(GetOrderByID_UC getOrderByOrderID, CancelOrder_UC cancelOrderUC, GetOrdersList_UC getOrdersListUC)
        {
            this.getOrderByOrderID = getOrderByOrderID;
            this.cancelOrderUC = cancelOrderUC;
            this.getOrdersListUC = getOrdersListUC;
        }

        [HttpGet]
        public async Task<IActionResult> OrderAdminHome(OrderStatus? status = null, int page = 1, CancellationToken ct = default)
        {
            var input = new InputGetOrdersList(
                StatusFilter: status,
                PageNumber: page,
                PageSize: 10
            );

            var result = await getOrdersListUC.HandleAsync(input, ct);

            ViewBag.StatusFilter = status;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = result.TotalPages;
            ViewBag.HasPreviousPage = result.HasPreviousPage;
            ViewBag.HasNextPage = result.HasNextPage;

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> OrderGet(int id, CancellationToken ct)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "ID đơn hàng không hợp lệ";
                return RedirectToAction("OrderAdminHome");
            }

            var order = await getOrderByOrderID.HandleAsync(new InputGetOrderByID(id), ct);

            if (order == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy đơn hàng";
                return RedirectToAction("OrderAdminHome");
            }

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> CancelOrder(int orderId, string? cancelReason, CancellationToken ct)
        {
            if (orderId <= 0)
            {
                TempData["ErrorMessage"] = "ID đơn hàng không hợp lệ";
                return RedirectToAction("OrderGet", new { id = orderId });
            }

            var result = await cancelOrderUC.HandleAsync(new InputCancelOrder(orderId, cancelReason), ct);

            if (result.Success)
            {
                TempData["SuccessMessage"] = result.Message;
            }
            else
            {
                TempData["ErrorMessage"] = result.Message;
            }

            return RedirectToAction("OrderGet", new { id = orderId });
        }
    }
}
