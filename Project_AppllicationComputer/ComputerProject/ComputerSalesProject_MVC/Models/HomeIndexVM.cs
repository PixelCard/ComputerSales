using ComputerSalesProject_MVC.Models.Product_ViewModel;

namespace ComputerSalesProject_MVC.Models
{
    public sealed class HomeIndexVM
    {
        public IEnumerable<ProductCardVM> Featured { get; set; } = Enumerable.Empty<ProductCardVM>();
        public IEnumerable<ProductListItemVM> NewArrivals { get; set; } = Enumerable.Empty<ProductListItemVM>();
        //public IEnumerable<ProductCardVM> BestSellingVariants { get; set; } = Enumerable.Empty<ProductCardVM>();
        public IEnumerable<ProductCardVM> BestSellingVariants { get; set; } = Enumerable.Empty<ProductCardVM>();


    }

}
