namespace ComputerSalesProject_MVC.Models
{
    public class ProductViewModel
    {
        public long ProductID { get; set; }

        public string SKU { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;

        public string ShortDescription { get; set; } = string.Empty;
        public string Status { get; set; } = "Active";   // dùng string để dễ binding ra View

        // Thông tin liên kết (chỉ lấy ID hoặc Name, không cần navigation đầy đủ)
        public long AccessoriesID { get; set; }
        public string? AccessoriesName { get; set; }

        public long ProviderID { get; set; }
        public string? ProviderName { get; set; }

        // Nếu cần hiển thị danh sách variants
        public List<string> Variants { get; set; } = new List<string>();
    }
}
