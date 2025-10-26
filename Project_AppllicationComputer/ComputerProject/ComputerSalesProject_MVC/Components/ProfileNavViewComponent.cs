using ComputerSales.Application.UseCase.Customer_UC;
using ComputerSales.Application.UseCaseDTO.Customer_DTO.getCustomerByUserID;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ComputerSalesProject_MVC.Components
{
    public class ProfileNavViewComponent : ViewComponent
    {
        private readonly getCustomerByUserID _getByUserId;
        public ProfileNavViewComponent(getCustomerByUserID getByUserId) => _getByUserId = getByUserId;

        public async Task<IViewComponentResult> InvokeAsync(CancellationToken ct = default)
        {
            string? avatar = null, name = null;
            var uid = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(uid, out var userId))
            {
                var dto = await _getByUserId.HandleAsync(new CustomerGetCustomerByUserID_Request(userId), ct);
                avatar = dto?.IMG; name = dto?.Name;
            }
            ViewBag.AvatarUrl = avatar;
            ViewBag.CustomerName = name;
            return View(); // Views/Shared/Components/ProfileNav/Default.cshtml
        }
    }
}
