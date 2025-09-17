using ComputerSales.Application.UseCase.Cart_UC.Commands.AddProtectionPlan;
using ComputerSales.Application.UseCase.Cart_UC.Commands.RemoveItem;
using ComputerSales.Application.UseCase.Cart_UC.Commands.UpdateQuantity;
using ComputerSales.Application.UseCase.Cart_UC.Queries.GetCartPage;
using Microsoft.AspNetCore.Mvc;

namespace ComputerSalesProject_MVC.Controllers
{
    public class CartController : Controller
    {
        private readonly GetCartPageQueryHandler _get;
        private readonly UpdateQuantityCommandHandler _upd;
        private readonly RemoveItemCommandHandler _rm;
        private readonly AddProtectionPlanCommandHandler _add;

        public CartController(
            GetCartPageQueryHandler get, 
            UpdateQuantityCommandHandler upd, 
            RemoveItemCommandHandler rm, 
            AddProtectionPlanCommandHandler add)
        {
            _get = get;
            _upd = upd;
            _rm = rm;
            _add = add;
        }

        public async Task<IActionResult> CartHome(CancellationToken ct)
        {
            int userId = int.Parse(User.FindFirst("uid")?.Value ?? "1"); //Tìm claim đầu tiên có type "uid".
            var vm = await _get.Handle(new GetCartPageQuery(userId), ct);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQty(int cartId, int itemId, int qty, CancellationToken ct)
        { await _upd.Handle(new UpdateQuantityCommand(cartId, itemId, qty), ct); return RedirectToAction(nameof(CartHome)); }

        [HttpPost]
        public async Task<IActionResult> Remove(int cartId, int itemId, CancellationToken ct)
        { await _rm.Handle(new RemoveItemCommand(cartId, itemId), ct); return RedirectToAction(nameof(CartHome)); }

        [HttpPost]
        public async Task<IActionResult> AddProtection(int cartId, int parentItemId, int planVariantId, CancellationToken ct)
        { await _add.Handle(new AddProtectionPlanCommand(cartId, parentItemId, planVariantId), ct); return RedirectToAction(nameof(CartHome)); }
    }
}
