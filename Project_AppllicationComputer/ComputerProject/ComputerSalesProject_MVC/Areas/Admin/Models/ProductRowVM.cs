using ComputerSales.Domain.Entity.EProduct;

namespace ComputerSalesProject_MVC.Areas.Admin.Models
{
    public class ProductRowVM
    {
        public long ProductID { get; set; }
        public string SKU { get; set; } = "";
        public string Slug { get; set; } = "";
        public string ShortDescription { get; set; } = "";
        public ProductStatus Status { get; set; }
        public int VariantsCount { get; set; }
        public string ProviderName { get; set; } = "";
        public string AccessoriesName { get; set; } = "";
    }
}
