using Microsoft.AspNetCore.Mvc;

namespace ComputerSalesProject_MVC.Areas.Admin.Controllers
{
    public class AdminHomeController : Controller
    {
        [Area("Admin")]
        public IActionResult AdminLayout()
        {
            return View();
        }
    }
}
