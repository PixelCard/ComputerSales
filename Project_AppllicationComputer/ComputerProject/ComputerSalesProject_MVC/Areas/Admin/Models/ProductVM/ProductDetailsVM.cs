using ComputerSales.Domain.Entity.EProduct;
using ComputerSalesProject_MVC.Areas.Admin.Models.NewFolder;

namespace ComputerSalesProject_MVC.Areas.Admin.Models.ProductVM
{
    public class ProductDetailsVM
    {
        public long ProductID { get; set; }
        public string ShortDescription { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public ProductStatus Status { get; set; }

        public string ProviderName { get; set; } = string.Empty;
        public string AccessoriesName { get; set; } = string.Empty;

        public List<ProductVariantDetailVM> Variants { get; set; } = new();
       
    }
}
