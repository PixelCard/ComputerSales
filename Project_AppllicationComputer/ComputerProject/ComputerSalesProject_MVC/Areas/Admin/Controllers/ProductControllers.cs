using ComputerSales.Application.Interface.Product_Interface;
using ComputerSales.Application.UseCase.Product_UC;
using ComputerSales.Application.UseCaseDTO.Product_DTO;
using ComputerSales.Infrastructure.Repositories.Product_Respo;
using Microsoft.AspNetCore.Mvc;

namespace ComputerSales.MVC.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly CreateProduct_UC _createUC; // liên kết usecase create Product 
        private readonly IProductRespository _productRepository;

        public ProductController(CreateProduct_UC createUC, IProductRespository productRepository) //tạo constructor rỗng để inject usecase vào
        {
            _createUC = createUC;
            _productRepository = productRepository;
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductDTOInput input, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            var result = await _createUC.HandleAsync(input, ct);
            // Dùng TempData thay vì ViewBag
            TempData["SuccessMessage"] = $"Sản phẩm {result.SKU} đã được tạo thành công!";

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var products = await _productRepository.GetAllProductsAsync(ct);

            var model = products.Select(p => p.ToResult());

            return View(model);
        }

    }
}
