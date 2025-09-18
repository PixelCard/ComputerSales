public class ProductCardVM
{
    public int ProductId { get; set; }
    public string SKU { get; set; } = "";
    public string Name { get; set; } = "";
    public string ShortDescription { get; set; } = "";

    public decimal Price { get; set; }
    public decimal DiscountPrice { get; set; }

    public List<string> Images { get; set; } = new();
    public List<VariantVM> Variants { get; set; } = new();
}

public class VariantVM
{
    public int VariantId { get; set; }
    public string SKU { get; set; } = "";
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal DiscountPrice { get; set; }
    public List<string> Images { get; set; } = new();
}
