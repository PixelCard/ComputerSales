using ComputerSalesProject_MVC.Areas.Admin.Models.VariantOptionValue;

namespace ComputerSalesProject_MVC.Areas.Admin.Models.ProductVariantVM
{
    public class OptionValueIndexVM
    {
        public long OptionTypeId { get; set; }
        public string OptionTypeCode { get; set; } = "";
        public string OptionTypeName { get; set; } = "";
        public long? VariantId { get; set; }

        public string? Query { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)Math.Max(1, TotalItems) / Math.Max(1, PageSize));

        public System.Collections.Generic.List<OptionValueRowVM> Items { get; set; } =
            new System.Collections.Generic.List<OptionValueRowVM>();
    }
}
