namespace ComputerSalesProject_MVC.Areas.Admin.Models.ProductVM
{
    public class ProductVM
    {
         
        public long? ProductId { get; set; }
        public string ProductName { get; set; }
        public string ImageUrl { get; set; }
        public int Quantity { get; set; }
        public decimal Revenue { get; set; }
    }
}
