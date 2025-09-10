using ComputerSalesProject_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ComputerSales.Domain.Entity;
using ComputerSales.Domain.Entity.EProduct;
using ComputerSales.Application.UseCase;
using ComputerSales.Application.UseCase.ProductProtection_UC;
using ComputerSales.Application.UseCaseDTO.ProductProtection_DTO.GetByIdDTO;
using System.Reflection.Metadata.Ecma335;

namespace ComputerSalesProject_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

       

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
