using ComputerSales.Application.UseCase.Cart_UC.Commands.AddCart;
using ComputerSales.Application.UseCase.Cart_UC.Commands.RemoveItem;
using ComputerSales.Application.UseCase.Cart_UC.Commands.UpdateQuantity;
using ComputerSales.Application.UseCase.Cart_UC.Queries.GetCartPage;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ComputerSalesProject_MVC.Controllers
{
    public class CartController : Controller
    {
        private readonly GetCartPageQueryHandler _get;
        private readonly UpdateQuantityCommandHandler _upd;
        private readonly RemoveItemCommandHandler _rm;
        private readonly AddItemCommandHandler _addItem;

        public CartController(
            GetCartPageQueryHandler get, 
            UpdateQuantityCommandHandler upd, 
            RemoveItemCommandHandler rm, 
            AddItemCommandHandler addItem
            )
        {
            _get = get;
            _upd = upd;
            _rm = rm;
            _addItem = addItem;
        }

        [HttpGet]
        public async Task<IActionResult> Count(CancellationToken ct)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            if (userId == 0)
            {
                return Json(new { count = 0 });
            }

          var vm = await _get.Handle(new GetCartPageQuery(userId), ct);
            return Json(new { count = vm.ItemsCount });
        }

        public async Task<IActionResult> CartHome(CancellationToken ct)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");//Tìm claim đầu tiên có type "NameIdentifier".

            if (userId == 0)
            {
                return RedirectToAction("Login", "Account");
            } 

            var vm = await _get.Handle(new GetCartPageQuery(userId), ct);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQty(int cartId, int itemId, int qty, CancellationToken ct)
        {
            qty = Math.Clamp(qty, 1, 3); 

            await _upd.Handle(new UpdateQuantityCommand(cartId, itemId, qty), ct);

            TempData["Info"] = "Quantity capped at 3 per item.";

            return RedirectToAction(nameof(CartHome)); 
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int cartId, int itemId, CancellationToken ct)
        { 
            await _rm.Handle(new RemoveItemCommand(cartId, itemId), ct); 

            return RedirectToAction(nameof(CartHome)); 
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(
            int productId,
            int? productVariantId,
            int qty = 1,
            int? optionalValueId = null,
            CancellationToken ct = default)
        {
            qty = Math.Clamp(qty, 1, 3);
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            if (userId == 0) return RedirectToAction("Login", "Account");

            await _addItem.Handle(
             new AddItemCommand(userId, productId, productVariantId, qty)
             {
                 OptionalValueId = optionalValueId   
             }, ct);

            return RedirectToAction(nameof(CartHome));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult checkout()
        {
            return RedirectToAction("OrderHome", "Order");
        }
    }
}
