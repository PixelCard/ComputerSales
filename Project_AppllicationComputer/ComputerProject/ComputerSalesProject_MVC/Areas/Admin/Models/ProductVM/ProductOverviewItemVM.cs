using ComputerSales.Domain.Entity.EProduct;

namespace ComputerSalesProject_MVC.Areas.Admin.Models.ProductVM
{
    public class ProductOverviewItemVM
    {
        public int ProductOverviewId { get; set; }
        public long ProductId { get; set; }
        public string TextContent { get; set; } = "";
        public DateTime CreateDate { get; set; }
    }
}
