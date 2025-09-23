using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

public sealed class ProductCreateVM
{
    [Required] public long AccessoriesId { get; set; }
    [Required] public long ProviderId { get; set; }

    [Required, StringLength(64)] public string SKU { get; set; } = null!;
    [Required, StringLength(128)] public string Slug { get; set; } = null!;
    [Required, StringLength(500)] public string ShortDescription { get; set; } = null!;

    // chọn nhiều OptionType (CPU, RAM…)
    public List<int> OptionTypeIds { get; set; } = new();

    // Nested
    public List<ProductVariantVM> Variants { get; set; } = new();

    // (optional) Overview
    public ProductOverviewVM? Overview { get; set; }

    // Dữ liệu cho dropdown UI
    public List<SelectListItem> AccessoriesOptions { get; set; } = new();
    public List<SelectListItem> ProviderOptions { get; set; } = new();
    public List<SelectListItem> OptionTypeOptions { get; set; } = new();
}