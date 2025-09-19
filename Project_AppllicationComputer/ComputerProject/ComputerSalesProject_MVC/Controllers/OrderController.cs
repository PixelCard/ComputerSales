using Microsoft.AspNetCore.Mvc;

namespace ComputerSalesProject_MVC.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
