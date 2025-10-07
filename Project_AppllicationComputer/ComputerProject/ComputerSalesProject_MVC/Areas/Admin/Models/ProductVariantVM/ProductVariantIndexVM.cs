using ComputerSales.Application.UseCaseDTO.ProductVariant_DTO;

namespace ComputerSalesProject_MVC.Areas.Admin.Models.NewFolder
{
    public class ProductVariantIndexVM
    {
        public List<ProductVariantOutput> Items { get; set; } = new();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public long ProductId { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / Math.Max(1, PageSize));
    }
}
