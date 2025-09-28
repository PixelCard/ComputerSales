using ComputerSales.Application.Interface.Product_Interface;
using ComputerSales.Application.UseCase.Product_UC;
using ComputerSales.Application.UseCaseDTO.Product_DTO;
using ComputerSales.Infrastructure.Repositories; // repo Provider
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ComputerSalesProject_MVCA.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly CreateProduct_UC _createUC;
        private readonly IProductRespository _productRepository;
        private readonly ProviderRepository _providerRepository;

        public ProductController(
            CreateProduct_UC createUC,
            IProductRespository productRepository,
            ProviderRepository providerRepository)
        {
            _createUC = createUC;
            _productRepository = productRepository;
            _providerRepository = providerRepository;
        }

        private async Task LoadProvidersAsync(long selectedProviderId = 0, CancellationToken ct = default)
        {
            var providers = await _providerRepository.GetAllProvidersAsync(ct);

            ViewBag.Providers = providers
                .Select(p => new SelectListItem
                {
                    Value = p.ProviderID.ToString(),
                    Text = p.ProviderName,
                    Selected = (p.ProviderID == selectedProviderId)
                })
                .ToList();
        }

        // GET: /Admin/Product/Create
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            await LoadProvidersAsync(0, ct);
            return View(new ProductDTOInput(
                ShortDescription: "",
                Status: 1,
                AccessoriesID: 0,
                ProviderID: 0,
                Slug: "",
                SKU: ""
            ));
        }

        // POST: /Admin/Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductDTOInput input, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await LoadProvidersAsync(input.ProviderID, ct);
                return View(input);
            }

            var result = await _createUC.HandleAsync(input, ct);
            TempData["SuccessMessage"] = $"Sản phẩm {result.SKU} đã được tạo thành công!";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Admin/Product
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var products = await _productRepository.GetAllProductsAsync(ct);
            var model = products.Select(p => p.ToResult());
            return View(model);
        }
    }
}
