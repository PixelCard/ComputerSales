using ComputerSales.Domain.Entity.EProduct;
using ComputerSalesProject_MVC.Models;

public sealed class ProductDetailViewModel
{
    // ----- Product core -----
    public long ProductId { get; set; }
    public string SKU { get; set; } = "";
    public string Title { get; set; } = "";
    public string ShortDescription { get; set; } = "";
    public ProductStatus Status { get; set; }        // dùng enum ProductStatus thay vì int
    public bool IsDeleted { get; set; }
    public long? AccessoriesID { get; set; }
    public long? ProviderID { get; set; }

    // ----- Variant đang chọn -----
    public int VariantId { get; set; }
    public string VariantSku { get; set; } = "";
    public int Quantity { get; set; }
    public bool InStock => Quantity > 0;
    public string? OverviewText { get; set; }

    // ----- Giá hiện hành -----
    public string Currency { get; set; } = "$";
    public decimal Price { get; set; }
    public decimal? OldPrice { get; set; }
    public PriceRowVM? CurrentPriceRaw { get; set; }
    public List<PriceRowVM> PriceHistory { get; set; } = new();

    // ----- Ảnh -----
    public List<string> Images { get; set; } = new();

    // ----- Option Groups -----
    public List<OptionGroupVM> OptionGroups { get; set; } = new();

    // ----- Variants khác -----
    public List<VariantSummaryVM> Variants { get; set; } = new();

    // ----- Overview blocks -----
    public List<OverviewBlockVM> OverviewBlocks { get; set; } = new();
}

// ========== Sub-VMs ==========




public sealed class OptionGroupVM
{
    public string Name { get; set; } = "";
    public List<OptionItemVM> Items { get; set; } = new();
}

public sealed class OptionItemVM
{
    public int Id { get; set; }
    public string Label { get; set; } = "";
    public bool Selected { get; set; }
    public bool Disabled { get; set; }
}

public enum OverviewBlockType
{
    Text = 0,
    List = 1,
    Image = 2,
    Logo = 3,
    Table = 4
}

