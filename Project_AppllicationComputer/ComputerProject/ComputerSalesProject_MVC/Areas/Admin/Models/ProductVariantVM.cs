using System.ComponentModel.DataAnnotations;

public sealed class ProductVariantVM
{
    [Required, StringLength(64)] public string SKU { get; set; } = null!;
    [Range(0, int.MaxValue)] public int Quantity { get; set; } = 0;

    // nếu cần gắn OptionValue cho variant
    public List<int> OptionValueIds { get; set; } = new();

    // nếu cần upload ảnh variant
    public List<IFormFile>? Images { get; set; }
}