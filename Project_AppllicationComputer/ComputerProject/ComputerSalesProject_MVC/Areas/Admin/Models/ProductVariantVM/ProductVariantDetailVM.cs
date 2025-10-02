using ComputerSales.Domain.Entity.EProduct;

namespace ComputerSalesProject_MVC.Areas.Admin.Models.NewFolder
{
    public class ProductVariantDetailVM
    {
        public int Id { get; set; }
        public long ProductId { get; set; }
        public string SKU { get; set; }
        public string VariantName { get; set; }
        public int Quantity { get; set; }
        public VariantStatus Status { get; set; }
    }
}
