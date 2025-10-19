using ComputerSales.Domain.Entity.EProduct;

namespace ComputerSalesProject_MVC.Areas.Admin.Models.ProductVM
{
    public class ProductOverviewItemVM
    {
        public int ProductOverviewId { get; set; }
        public long ProductId { get; set; }
        public OverviewBlockType BlockType { get; set; }
        public string TextContent { get; set; } = "";
        public string? ImageUrl { get; set; }
        public string? Caption { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
