using ComputerSales.Domain.Entity.EProduct;

namespace ComputerSales.Domain.Entity.EProvider
{
    public class Provider
    {
        public long ProviderID { get; set; }
        public string ProviderName { get; set; } = null!;

        // 1 - N Product
        public ICollection<Product> Products { get; set; } = new List<Product>();

        public static Provider create(string ProviderName) => new Provider
        {
            ProviderName= ProviderName,
        };       
    }
}
