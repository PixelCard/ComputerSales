using System.Collections.Generic;

namespace ComputerSalesProject_MVC.Models
{
    public class OrdersListViewModel
    {
        public List<OrderWithDetailsViewModel> Orders { get; set; } = new();
    }

    public class OrderWithDetailsViewModel
    {
        public int OrderID { get; set; }
        public DateTime OrderTime { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal GrandTotal { get; set; }
        public List<OrderDetailItemViewModel> Details { get; set; } = new();
    }

    public class OrderDetailItemViewModel
    {
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public string? ImageUrl { get; set; }
        public string? OptionSummary { get; set; }
    }
}


