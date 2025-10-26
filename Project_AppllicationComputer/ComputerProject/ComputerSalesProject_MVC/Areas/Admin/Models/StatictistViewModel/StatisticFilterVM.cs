namespace ComputerSalesProject_MVC.Areas.Admin.Models.StatictistViewModel
{
    public class StatisticFilterVM
    {
        public string Type { get; set; } = "month"; // "day", "month", "year"
        public int? Year { get; set; }
        public int? Month { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        // Kết quả sau khi lọc
        public List<StatisticResultVM> Results { get; set; } = new();
    }

    public class StatisticResultVM
    {
        public string Label { get; set; } = "";
        public decimal TotalRevenue { get; set; }
        public int OrderCount { get; set; }
    }
}
