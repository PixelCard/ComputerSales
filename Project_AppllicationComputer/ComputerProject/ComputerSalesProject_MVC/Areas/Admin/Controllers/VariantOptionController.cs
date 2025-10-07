using Azure.Core;
using ComputerSales.Application.UseCaseDTO.OptionalType_DTO;
using ComputerSales.Application.UseCaseDTO.OptionalValue_DTO;
using ComputerSales.Domain.Entity.EOptional;
using ComputerSales.Domain.Entity.EProduct;
using ComputerSales.Domain.Entity.EVariant;
using ComputerSales.Infrastructure.Persistence;
using ComputerSalesProject_MVC.Areas.Admin.Models.ProductVariantVM;
using ComputerSalesProject_MVC.Areas.Admin.Models.VariantOptionValue;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

[Area("Admin")]
[Route("Admin/[controller]/[action]")]
public class VariantOptionController : Controller
{
    private readonly AppDbContext _db;

    public VariantOptionController(AppDbContext db)
    {
        _db = db;
    }

    // ======================== OPTION TYPE LIST ======================== //
    [HttpGet]
    public async Task<IActionResult> Index(long variantId, CancellationToken ct)
    {
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

    // ======================== CREATE OPTION TYPE ======================== //
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
        if (!ModelState.IsValid) return View(input);

        var count = await _db.optionTypes.CountAsync(ct);
        var autoCode = $"OPT_{(count + 1).ToString("D2")}";

        var newType = new OptionType { Code = autoCode, Name = input.Name };
        _db.optionTypes.Add(newType);
        await _db.SaveChangesAsync(ct);

        _db.productOptionTypes.Add(new ProductOptionType { ProductId = productId, OptionTypeId = newType.Id });
        await _db.SaveChangesAsync(ct);

        TempData["Success"] = $"Thêm OptionType thành công! (Code = {autoCode})";
        return RedirectToAction(nameof(Index), new { variantId });
    }

    // ======================== OPTION VALUE INDEX ======================== //
    [HttpGet] // CHỈ GIỮ HttpGet, KHÔNG gắn route riêng để tránh chồng URL
    public async Task<IActionResult> IndexOptionValue(
        long optionTypeId,
        long? variantId,
        string? q,
        int page = 1,
        int pageSize = 20,
        CancellationToken ct = default)
    {
        if (page < 1) page = 1;
        if (pageSize <= 0 || pageSize > 200) pageSize = 20;

        var type = await _db.optionTypes.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == optionTypeId, ct);
        if (type == null) return NotFound($"Không tìm thấy OptionType #{optionTypeId}");

        var query = _db.optionalValues.AsNoTracking()
            .Where(v => v.OptionTypeId == optionTypeId);

        if (!string.IsNullOrWhiteSpace(q))
        {
            var key = q.Trim();
            query = query.Where(v => v.Value.Contains(key));
        }

        var total = await query.CountAsync(ct);

        HashSet<long> assignedIds = new();
        if (variantId.HasValue)
        {
            assignedIds = new HashSet<long>(
                await _db.variantOptionValues
                    .Where(v => v.VariantId == variantId.Value)
                    .Select(v => (long)v.OptionalValueId)
                    .ToListAsync(ct)
            );
        }


        var items = await query
            .OrderBy(v => v.SortOrder).ThenBy(v => v.Value)
            .Select(v => new OptionValueRowVM
            {
                Id = v.Id,
                OptionTypeId = v.OptionTypeId,
                Value = v.Value,
                SortOrder = v.SortOrder,
                Price = v.Price,
                IsAssigned = assignedIds.Contains(v.Id)
            })
            .Skip((page - 1) * pageSize).Take(pageSize)
            .ToListAsync(ct);

        var vm = new OptionValueIndexVM
        {
            OptionTypeId = type.Id,
            OptionTypeCode = type.Code,
            OptionTypeName = type.Name,
            VariantId = variantId,
            Items = items,
            Query = q,
            Page = page,
            PageSize = pageSize,
            TotalItems = total
        };

        return View(vm);
    }

    // ======================== CREATE OPTION VALUE ======================== //
    [HttpGet]
    public IActionResult CreateOptionValue(long optionTypeId, long variantId)
    {
        ViewBag.OptionTypeId = optionTypeId;
        ViewBag.VariantId = variantId;
        return View(new OptionalValueInput((int)optionTypeId, "", 0, 0));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateOptionValue(OptionalValueInput input, long variantId, CancellationToken ct)
    {
        var valueNorm = (input.Value ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(valueNorm))
        {
            ModelState.AddModelError(nameof(input.Value), "Giá trị không được để trống.");
            ViewBag.OptionTypeId = input.OptionTypeId;
            ViewBag.VariantId = variantId;
            return View(input);
        }

        var exists = await _db.optionalValues
            .AnyAsync(x => x.OptionTypeId == input.OptionTypeId && x.Value == valueNorm, ct);

        if (exists)
        {
            ModelState.AddModelError(nameof(input.Value), "Giá trị này đã tồn tại trong OptionType hiện tại.");
            ViewBag.OptionTypeId = input.OptionTypeId;
            ViewBag.VariantId = variantId;
            return View(input);
        }

        try
        {
            _db.optionalValues.Add(new OptionalValue
            {
                OptionTypeId = input.OptionTypeId,
                Value = valueNorm,
                SortOrder = input.SortOrder,
                Price = input.Price
            });

            await _db.SaveChangesAsync(ct);
            TempData["Success"] = "Thêm OptionValue thành công!";
            return RedirectToAction(nameof(IndexOptionValue),
                new { optionTypeId = input.OptionTypeId, variantId });
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError(nameof(input.Value),
                "Giá trị đã tồn tại (cập nhật đồng thời). Vui lòng thử lại.");
            ViewBag.OptionTypeId = input.OptionTypeId;
            ViewBag.VariantId = variantId;
            return View(input);
        }
    }

    // ======================== EDIT/DELETE OPTION TYPE & VALUE ======================== //
    [HttpGet]
    public async Task<IActionResult> EditOptionType(long id, long variantId, CancellationToken ct)
    {
        var type = await _db.optionTypes.FindAsync(new object[] { id }, ct);
        if (type == null) return NotFound();

        ViewBag.VariantId = variantId;
        ViewBag.ProductId = await _db.productVariants
            .Where(v => v.Id == variantId)
            .Select(v => v.ProductId)
            .FirstOrDefaultAsync(ct);

        return View(new OptionalTypeInput(type.Code, type.Name));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditOptionType(long id, long variantId, OptionalTypeInput input, CancellationToken ct)
    {
        var type = await _db.optionTypes.FindAsync(new object[] { id }, ct);
        if (type == null) return NotFound();

        if (!ModelState.IsValid)
        {
            ViewBag.VariantId = variantId;
            ViewBag.ProductId = await _db.productVariants.Where(v => v.Id == variantId).Select(v => v.ProductId).FirstOrDefaultAsync(ct);
            return View(input);
        }

        try
        {
            type.Name = input.Name?.Trim() ?? type.Name;
            _db.Update(type);
            await _db.SaveChangesAsync(ct);

            TempData["Success"] = "Cập nhật OptionType thành công.";
            return RedirectToAction(nameof(Index), new { variantId });
        }
        catch (DbUpdateException ex)
        {
            ModelState.AddModelError(string.Empty, $"Lỗi khi cập nhật: {ex.Message}");
            ViewBag.VariantId = variantId;
            ViewBag.ProductId = await _db.productVariants.Where(v => v.Id == variantId).Select(v => v.ProductId).FirstOrDefaultAsync(ct);
            return View(input);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteOptionType(long id, long variantId, CancellationToken ct)
    {
        var type = await _db.optionTypes.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (type == null)
        {
            TempData["Error"] = "OptionType không tồn tại.";
            return RedirectToAction(nameof(Index), new { variantId });
        }

        using var tx = await _db.Database.BeginTransactionAsync(ct);
        try
        {
            var values = await _db.optionalValues.Where(v => v.OptionTypeId == id).ToListAsync(ct);
            if (values.Count > 0) _db.optionalValues.RemoveRange(values);

            var links = await _db.productOptionTypes.Where(pot => pot.OptionTypeId == id).ToListAsync(ct);
            if (links.Count > 0) _db.productOptionTypes.RemoveRange(links);

            _db.optionTypes.Remove(type);

            await _db.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);

            TempData["Success"] = "Đã xoá OptionType và toàn bộ OptionValue liên quan.";
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync(ct);
            TempData["Error"] = $"Không thể xoá: {ex.Message}";
        }

        return RedirectToAction(nameof(Index), new { variantId });
    }

    [HttpGet]
    public async Task<IActionResult> EditOptionValue(long id, long variantId, CancellationToken ct)
    {
        var val = await _db.optionalValues.FindAsync(new object[] { id }, ct);
        if (val == null) return NotFound();

        ViewBag.VariantId = variantId;
        ViewBag.OptionTypeId = val.OptionTypeId;

        return View(new OptionalValueInput(val.OptionTypeId, val.Value, val.SortOrder, val.Price));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditOptionValue(long id, long variantId, OptionalValueInput input, CancellationToken ct)
    {
        var val = await _db.optionalValues.FindAsync(new object[] { id }, ct);
        if (val == null) return NotFound();

        if (!ModelState.IsValid)
        {
            ViewBag.VariantId = variantId;
            ViewBag.OptionTypeId = val.OptionTypeId;
            return View(input);
        }

        var valueNorm = (input.Value ?? string.Empty).Trim();
        var duplicate = await _db.optionalValues
            .AnyAsync(x => x.Id != id && x.OptionTypeId == val.OptionTypeId && x.Value == valueNorm, ct);
        if (duplicate)
        {
            ModelState.AddModelError(nameof(input.Value), "Giá trị đã tồn tại trong OptionType này.");
            ViewBag.VariantId = variantId;
            ViewBag.OptionTypeId = val.OptionTypeId;
            return View(input);
        }

        try
        {
            val.Value = valueNorm;
            val.SortOrder = input.SortOrder;
            val.Price = input.Price;

            _db.Update(val);
            await _db.SaveChangesAsync(ct);

            TempData["Success"] = "Cập nhật OptionValue thành công.";
            return RedirectToAction(nameof(IndexOptionValue), new { optionTypeId = val.OptionTypeId, variantId });
        }
        catch (DbUpdateException ex)
        {
            ModelState.AddModelError(string.Empty, $"Lỗi khi cập nhật: {ex.Message}");
            ViewBag.VariantId = variantId;
            ViewBag.OptionTypeId = val.OptionTypeId;
            return View(input);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteOptionValue(long id, long variantId, CancellationToken ct)
    {
        var val = await _db.optionalValues.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
        if (val == null) return NotFound();

        try
        {
            _db.optionalValues.Remove(new OptionalValue { Id = (int)id });
            await _db.SaveChangesAsync(ct);

            TempData["Success"] = "Đã xoá OptionValue.";
        }
        catch (DbUpdateException ex)
        {
            TempData["Error"] = $"Không thể xoá OptionValue: {ex.Message}";
        }

        return RedirectToAction(nameof(IndexOptionValue), new { optionTypeId = val.OptionTypeId, variantId });
    }

    // ======================== ASSIGN/UNASSIGN SINGLE ======================== //
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AssignValue(long optionValueId, long variantId, bool assign, long optionTypeId, CancellationToken ct)
    {
        if (optionTypeId == 0) // fallback
            optionTypeId = await _db.optionalValues.Where(x => x.Id == optionValueId)
                              .Select(x => (long)x.OptionTypeId).FirstOrDefaultAsync(ct);

        using var tx = await _db.Database.BeginTransactionAsync(ct);
        try
        {
            var exists = await _db.variantOptionValues
                .AnyAsync(v => v.VariantId == variantId && v.OptionalValueId == optionValueId, ct);

            if (assign)
            {
                if (!exists)
                {
                    _db.variantOptionValues.Add(new VariantOptionValue { VariantId = (int)variantId, OptionalValueId = (int)optionValueId });
                    TempData["Success"] = "Đã assign OptionValue thành công.";
                }
                else
                {
                    TempData["Info"] = "OptionValue đã được assign rồi.";
                }
            }
            else if (exists)
            {
                var rec = await _db.variantOptionValues
                    .FirstAsync(v => v.VariantId == variantId && v.OptionalValueId == optionValueId, ct);
                _db.variantOptionValues.Remove(rec);
                TempData["Success"] = "Đã unassign OptionValue thành công.";
            }
            else
            {
                TempData["Info"] = "OptionValue chưa được assign.";
            }

            await _db.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);

            return RedirectToAction(nameof(IndexOptionValue), new { optionTypeId, variantId });
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync(ct);
            TempData["Error"] = $"Lỗi khi cập nhật assign: {ex.Message}";
            return RedirectToAction(nameof(IndexOptionValue), new { optionTypeId, variantId });
        }
    }

    // ======================== ASSIGN MULTI (CHECKBOX) ======================== //
    [HttpGet]
    public async Task<IActionResult> AssignToVariant(long variantId, long optionTypeId, CancellationToken ct)
    {
        var variant = await _db.productVariants.FirstOrDefaultAsync(v => v.Id == variantId, ct);
        if (variant == null) return NotFound("Variant không tồn tại.");

        var allValues = await _db.optionalValues.AsNoTracking()
            .Where(ov => ov.OptionTypeId == optionTypeId)
            .OrderBy(ov => ov.SortOrder)
            .Select(ov => new { ov.Id, ov.Value, ov.Price, ov.SortOrder })
            .ToListAsync(ct);

        var assignedIds = await _db.variantOptionValues
            .Where(vov => vov.VariantId == variantId && vov.OptionalValue.OptionTypeId == optionTypeId)
            .Select(vov => vov.OptionalValueId)
            .ToListAsync(ct);

        var vm = new AssignToVariantVM
        {
            VariantId = variantId,
            OptionTypeId = optionTypeId,
            Items = allValues.Select(av => new AssignItemVM
            {
                Id = av.Id,
                Value = av.Value,
                Price = av.Price,
                SortOrder = av.SortOrder,
                IsAssigned = assignedIds.Contains(av.Id)
            }).ToList()
        };

        ViewBag.VariantId = variantId;
        ViewBag.OptionTypeName = await _db.optionTypes.Where(ot => ot.Id == optionTypeId)
            .Select(ot => ot.Name).FirstOrDefaultAsync(ct);

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AssignToVariant(long variantId, long optionTypeId, List<long> selectedIds, CancellationToken ct)
    {
        using var tx = await _db.Database.BeginTransactionAsync(ct);
        try
        {
            var oldAssigns = await _db.variantOptionValues
                .Where(vov => vov.VariantId == variantId && vov.OptionalValue.OptionTypeId == optionTypeId)
                .ToListAsync(ct);
            _db.variantOptionValues.RemoveRange(oldAssigns);

            foreach (var valueId in selectedIds)
                _db.variantOptionValues.Add(new VariantOptionValue { VariantId = (int)variantId, OptionalValueId = (int)valueId });

            await _db.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);

            TempData["Success"] = $"Đã cập nhật assign ({selectedIds.Count} values).";
            return RedirectToAction(nameof(IndexOptionValue),
                "VariantOption",
                new { area = "Admin", optionTypeId, variantId });
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync(ct);
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(AssignToVariant), new { variantId, optionTypeId });
        }
    }
}
