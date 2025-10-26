using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComputerSalesProject_MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminHomeController : Controller
    {
        [Area("Admin")]
        public IActionResult AdminLayout()
        {
            return View();
        }
    }
}
