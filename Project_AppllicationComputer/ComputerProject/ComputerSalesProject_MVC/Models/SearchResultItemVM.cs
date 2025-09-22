public sealed class SearchResultItemVM
{
    public int ProductId { get; set; }
    public string Title { get; set; } = "";
    public string SKU { get; set; } = "";
    public string? ImageUrl { get; set; }
    public decimal? Price { get; set; }
}

