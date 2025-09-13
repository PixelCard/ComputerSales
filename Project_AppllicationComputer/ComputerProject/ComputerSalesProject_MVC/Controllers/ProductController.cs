using Microsoft.AspNetCore.Mvc;

namespace ComputerSalesProject_MVC.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult ProductDetails()
        {
            return View();
        }
    }
}
