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
    public IActionResult CreateOptionType(long productId, long variantId)
    {
        ViewBag.ProductId = productId;
        ViewBag.VariantId = variantId;
        return View(new OptionalTypeInput("", ""));
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateOptionType(OptionalTypeInput input, long productId, long variantId, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return View(input);

        // ✅ Đếm số OptionType đã có trong DB
        var count = await _db.optionTypes.CountAsync(ct);

        // ✅ Sinh code tự động (Code_01, Code_02,...)
        var autoCode = $"OPT_{(count + 1).ToString("D2")}";

        var newType = new OptionType
        {
            Code = autoCode,     // ❌ không lấy từ input.Code
            Name = input.Name
        };

        _db.optionTypes.Add(newType);
        await _db.SaveChangesAsync(ct);

        // Gắn với Product
        _db.productOptionTypes.Add(new ProductOptionType
        {
            ProductId = productId,
            OptionTypeId = newType.Id
        });
        await _db.SaveChangesAsync(ct);

        TempData["Success"] = $"Thêm OptionType thành công! (Code = {autoCode})";
        return RedirectToAction("Index", new { variantId = variantId });
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
