namespace ComputerSalesProject_MVC.Models
{
    public sealed class HomeIndexVM
    {
        public IEnumerable<ProductCardVM> Featured { get; set; } = Enumerable.Empty<ProductCardVM>();
        public IEnumerable<ProductListItemVM> NewArrivals { get; set; } = Enumerable.Empty<ProductListItemVM>();
    }

}
