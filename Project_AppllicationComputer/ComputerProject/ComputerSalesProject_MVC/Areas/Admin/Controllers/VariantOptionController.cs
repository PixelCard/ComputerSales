using Azure.Core;
using ComputerSales.Application.UseCaseDTO.OptionalType_DTO;
using ComputerSales.Application.UseCaseDTO.OptionalValue_DTO;
using ComputerSales.Domain.Entity.EOptional;
using ComputerSales.Domain.Entity.EProduct;
using ComputerSales.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Area("Admin")]
[Route("Admin/[controller]/[action]")]
public class VariantOptionController : Controller
{
    private readonly AppDbContext _db;

    public VariantOptionController(AppDbContext db)
    {
        _db = db;
    }

    // Danh sách OptionType của 1 Variant
    [HttpGet]
    public async Task<IActionResult> Index(long variantId, CancellationToken ct)
    {
        // Lấy các OptionType gắn với Product của Variant này
        var variant = await _db.productVariants
            .Include(v => v.Product)
            .FirstOrDefaultAsync(v => v.Id == variantId, ct);

        if (variant == null) return NotFound();

        var optionTypes = await (from pot in _db.productOptionTypes
                                 join ot in _db.optionTypes on pot.OptionTypeId equals ot.Id
                                 where pot.ProductId == variant.ProductId
                                 select ot).ToListAsync(ct);

        ViewBag.VariantId = variantId;
        ViewBag.ProductId = variant.ProductId;

        return View(optionTypes);
    }

    // Tạo mới OptionType cho Product (sau đó Variant sẽ dùng được)
    [HttpGet]
    public IActionResult CreateOptionType(long productId)
    {
        ViewBag.ProductId = productId;
        return View(new OptionalTypeInput("", "")); // Code, Name
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateOptionType(OptionalTypeInput input, long productId, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return View(input);

        // Tạo OptionType mới
        var newType = new OptionType
        {
            Code = input.Code,
            Name = input.Name
        };
        _db.optionTypes.Add(newType);
        await _db.SaveChangesAsync(ct);

        // Gắn OptionType với Product
        _db.productOptionTypes.Add(new ProductOptionType
        {
            ProductId = productId,
            OptionTypeId = newType.Id
        });
        await _db.SaveChangesAsync(ct);

        TempData["Success"] = "Thêm OptionType thành công!";
        return RedirectToAction("Index", new { variantId = Request.Query["variantId"] });
    }

    // Thêm OptionValue cho OptionType
    [HttpGet]
    public IActionResult CreateOptionValue(long optionTypeId)
    {
        ViewBag.OptionTypeId = optionTypeId;
        return View(new OptionalValueInput((int)optionTypeId, "", 0, 0));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateOptionValue(OptionalValueInput input, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return View(input);

        _db.optionalValues.Add(new OptionalValue
        {
            OptionTypeId = input.OptionTypeId,
            Value = input.Value,
            SortOrder = input.SortOrder,
            Price = input.Price
        });
        await _db.SaveChangesAsync(ct);

        TempData["Success"] = "Thêm OptionValue thành công!";
        return RedirectToAction("Index", new { variantId = Request.Query["variantId"] });
    }
}
