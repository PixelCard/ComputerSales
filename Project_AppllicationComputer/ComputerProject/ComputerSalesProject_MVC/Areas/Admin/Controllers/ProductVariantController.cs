using ComputerSales.Application.UseCase.ProductVariant_UC;
using ComputerSales.Application.UseCaseDTO.ProductVariant_DTO;
using ComputerSales.Application.UseCaseDTO.ProductVariant_DTO.DeleteDTO_ProductVariant;
using ComputerSales.Application.UseCaseDTO.ProductVariant_DTO.GetByIdDTO;
using ComputerSales.Application.UseCaseDTO.ProductVariant_DTO.UpdateDTO;
using ComputerSales.Domain.Entity.EProduct;
using ComputerSales.Infrastructure.Persistence;
using ComputerSalesProject_MVC.Areas.Admin.Models.NewFolder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ComputerSalesProject_MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class ProductVariantController : Controller
    {
        private readonly ILogger<ProductVariantController> _logger;
        private readonly AppDbContext _db;
        private readonly CreateProductVariant_UC _create;
        private readonly GetByIdProductVariant_UC _get;
        private readonly UpdateProductVariant_UC _update;
        private readonly DeleteProductVariant_UC _delete;

        public ProductVariantController(
            AppDbContext db,
             ILogger<ProductVariantController> logger,
            CreateProductVariant_UC create,
            GetByIdProductVariant_UC get,
            UpdateProductVariant_UC update,
            DeleteProductVariant_UC delete)
        {
            _db = db;        
            _create = create;
            _get = get;
            _update = update;
            _delete = delete;
            _logger = logger;
        }


        private void LoadVariantStatusLookups()
        {
            ViewBag.VariantStatusList = new SelectList(new[]
            {
                new { Value = (int)VariantStatus.Inactive, Text = nameof(VariantStatus.Inactive) },
                new { Value = (int)VariantStatus.Active,   Text = nameof(VariantStatus.Active) }
            }, "Value", "Text");
        }

        [HttpGet]
        public IActionResult Create(long productId)
        {
            LoadVariantStatusLookups();
            return View(new ProductVariantInput(productId, "", VariantStatus.Active, 0, ""));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductVariantInput input, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                LoadVariantStatusLookups();
                return View(input);
            }

            var productExists = await _db.Products.AnyAsync(p => p.ProductID == input.ProductId, ct);
            if (!productExists)
            {
                ModelState.AddModelError("", "Sản phẩm không tồn tại, không thể thêm biến thể.");
                LoadVariantStatusLookups();
                return View(input);
            }

            var rs = await _create.HandleAsync(input, ct);
            TempData["Success"] = "Tạo biến thể thành công.";
            return RedirectToAction("Index", "Product", new { area = "Admin" });
        }



        [HttpGet("{id:long}")]
        public async Task<IActionResult> UpdateProductVariant(long id, CancellationToken ct)
        {
            var rs = await _get.HandleAsync(new GetById_ProductVariant_DTOcs((int)id), ct);
            // Debug console
            Console.WriteLine("\n====================================");
            Console.WriteLine($"🚨 DEBUG GET => VariantId={rs?.Id}, ProductId={rs?.ProductId}");
            Console.WriteLine("====================================\n"); if (rs == null) return NotFound();

            LoadVariantStatusLookups();

            // Truyền đầy đủ ProductId sang View (để giữ trong hidden field)
            var vm = new UpdateDTO_ProductVariant(
                rs.Id,
                rs.ProductId,
                rs.SKU,
                rs.Status,
                rs.Quantity,
                rs.VariantName
            );

            return View(vm);
        }

        [HttpPost("{id:long}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProductVariant(long id, UpdateDTO_ProductVariant input, CancellationToken ct)
        {
            Console.WriteLine("\n====================================");
            Console.WriteLine($"🚨 DEBUG POST => Id={input.Id}, ProductId={input.ProductId}");
            Console.WriteLine("====================================\n");

            // Debug logger
            _logger.LogWarning("🚨 DEBUG POST => Id={Id}, ProductId={ProductId}", input.Id, input.ProductId); if (!ModelState.IsValid)
            {
                LoadVariantStatusLookups();
                return View(input);
            }

            if (id != input.Id)
            {
                return BadRequest("ID không khớp.");
            }

            var productExists = await _db.Products.AnyAsync(p => p.ProductID == input.ProductId, ct);
            if (!productExists)
            {
                ModelState.AddModelError("", $"Sản phẩm với ID {input.ProductId} không tồn tại.");
                LoadVariantStatusLookups();
                return View(input);
            }

            var rs = await _update.HandleAsync(input, ct);
            if (rs == null) return NotFound();

            TempData["Success"] = "Cập nhật biến thể thành công.";

            // ✅ quay về Index của ProductVariant (theo ProductId cha)
            return RedirectToAction("Index", new { productId = input.ProductId });
        }



        [HttpGet("Delete/{id:long}")]
        public async Task<IActionResult> DeleteProductVariant(long id, CancellationToken ct)
        {
            var rs = await _get.HandleAsync(new GetById_ProductVariant_DTOcs((int)id), ct);
            if (rs == null) return NotFound();

            var vm = new ProductVariantDetailVM
            {
                Id = rs.Id,
                SKU = rs.SKU,
                VariantName = rs.VariantName,
                Quantity = rs.Quantity,
                Status = rs.Status
            };

            return View(vm);
        }



        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var deleted = await _delete.HandleAsync(new ProductVariantDelete_DTO(id), ct);
            if (deleted == null) return NotFound(); // ✅ check null thay vì !ok

            TempData["Success"] = "Xóa biến thể thành công.";
            return RedirectToAction("Index", "Product", new { area = "Admin" });
        }
        

        [HttpGet]
        public async Task<IActionResult> Index(long productId, int page = 1, int pageSize = 10, CancellationToken ct = default)
        {
            var query = _db.productVariants.Where(v => v.ProductId == productId);

            var totalItems = await query.CountAsync(ct);

            var variants = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(v => new ProductVariantOutput(
                    v.SKU,
                    v.Status,
                    v.ProductId,
                    v.Quantity,
                    v.Id,
                    v.VariantName
                ))
                .ToListAsync(ct);

            var vm = new ProductVariantIndexVM
            {
                Items = variants,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                ProductId = productId
            };

            return View(vm);
        }





    }
}
