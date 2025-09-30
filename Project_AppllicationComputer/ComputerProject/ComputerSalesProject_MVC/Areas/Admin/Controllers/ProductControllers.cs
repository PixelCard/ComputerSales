using ComputerSales.Application.UseCase.Product_UC;
using ComputerSales.Application.UseCaseDTO.Product_DTO;
using ComputerSales.Domain.Entity.EProduct; // ProductStatus
using ComputerSales.Infrastructure.Persistence; // AppDbContext
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ComputerSalesProject_MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _db;
        private readonly CreateProduct_UC _createUC;

        public ProductController(AppDbContext db, CreateProduct_UC createUC)
        {
            _db = db;
            _createUC = createUC;
        }

        // Nạp dropdown cho View (Providers, Accessories, Status=int)
        private async Task LoadLookupsAsync(CancellationToken ct = default)
        {
            var providers = await _db.Providers
                .AsNoTracking()
                .Select(x => new { x.ProviderID, x.ProviderName })
                .ToListAsync(ct);

            var accessories = await _db.accessories
                .AsNoTracking()
                .Select(x => new { x.AccessoriesID, x.Name })
                .ToListAsync(ct);

            ViewBag.ProviderList = new SelectList(providers, "ProviderID", "ProviderName");
            ViewBag.AccessoriesList = new SelectList(accessories, "AccessoriesID", "Name");
            ViewData["DbgCounts"] = $"Providers={providers.Count}, Accessories={accessories.Count}";

            // Status: enum -> int cho View (DTO nhận int)
            ViewBag.StatusList = new SelectList(new[]
            {
                new { Value = (int)ProductStatus.Inactive, Text = nameof(ProductStatus.Inactive) },
                new { Value = (int)ProductStatus.Active,   Text = nameof(ProductStatus.Active) }
            }, "Value", "Text");
        }

        // GET /Admin/Product/Create
        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            await LoadLookupsAsync(ct);
            return View(); // @model ProductDTOInput
        }

        // POST /Admin/Product/Create (tạo qua form)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductDTOInput input, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await LoadLookupsAsync(ct);
                return View(input);
            }

            try
            {
                // Gọi thẳng UseCase, không chỉnh DTO/UC
                ProductOutputDTOcs output = await _createUC.HandleAsync(input, ct);

                TempData["Success"] = "Tạo sản phẩm thành công.";
                // output.ProductID là khóa chính theo model bạn đưa
                return RedirectToAction(nameof(Detail), new { id = output.ProductID });
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Lỗi: {ex.Message}");
            }

            await LoadLookupsAsync(ct);
            return View(input);
        }

        // Endpoint JSON (tùy chọn) – tạo bằng body JSON, trả Output
        [HttpPost]
        [Produces("application/json")]
        public async Task<IActionResult> CreateJson([FromBody] ProductDTOInput input, CancellationToken ct)
        {       
            if (!ModelState.IsValid) return BadRequest(ModelState);

            ProductOutputDTOcs output = await _createUC.HandleAsync(input, ct);
            return CreatedAtAction(nameof(Detail), new { id = output.ProductID }, output);
        }

        // Xem nhanh chi tiết sau khi tạo (không gọi UseCase khác)
        [HttpGet]
        public async Task<IActionResult> Detail(long id, CancellationToken ct)
        {
            var p = await _db.Products
                .AsNoTracking()
                .Include(x => x.Provider)
                .Include(x => x.Accessories)
                .FirstOrDefaultAsync(x => x.ProductID == id, ct);

            if (p == null) return NotFound();

            return View(p);
        }
    }
}
