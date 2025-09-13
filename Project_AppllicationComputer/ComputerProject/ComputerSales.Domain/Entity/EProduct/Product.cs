using ComputerSales.Domain.Entity.ECategory;
using ComputerSales.Domain.Entity.EProvider;

namespace ComputerSales.Domain.Entity.EProduct
{
    public enum ProductStatus
    {
        Inactive = 0,
        Active = 1
    }
    public class Product
    {
            // EF cần ctor không tham số: dùng protected để tránh CS0122 khi map ở project khác
            protected Product() { }

            public long ProductID { get; set; }

            // Nên có mã hiển thị/SEO
            public string SKU { get; set; } = null!;
            public string Slug { get; set; } = null!;     // ví dụ: mloong-ryzen-5-5500-rx6600

            public string ShortDescription { get; set; } = null!;
            public ProductStatus Status { get; set; } = ProductStatus.Active;

            // Soft delete + concurrency
            public bool IsDeleted { get; set; }
            public byte[] RowVersion { get; set; } = Array.Empty<byte>();

            // FKs
            public long AccessoriesID { get; set; }       // (nếu là Category thì rename: CategoryID)
            public long ProviderID { get; set; }          // Brand/Vendor

            // Navigations
            public Accessories Accessories { get; set; } = null!;
            public Provider Provider { get; set; } = null!;
            public ProductOverview? ProductOverview { get; set; }           // 1-1
            public ProductProtection? ProductProtection { get; set; }       // 1-1

            public ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();   // 1-n
            public ICollection<ProductOptionType> ProductOptionTypes { get; set; } = new List<ProductOptionType>(); // n-n (CPU, RAM, …)

            // Factory để giữ bất biến
            public static Product Create(long accessoriesId, long providerId, string shortDescription, string sku, string slug)
                => new Product
                {
                    AccessoriesID = accessoriesId,
                    ProviderID = providerId,
                    ShortDescription = shortDescription,
                    SKU = sku,
                    Slug = slug,
                    Status = ProductStatus.Active,
                    IsDeleted = false
                };
    }
}
