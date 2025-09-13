using ComputerSales.Domain.Entity.EProduct;

namespace ComputerSales.Domain.Entity.ECategory
{
    public class Accessories
    {
        public long AccessoriesID { get; set; }
        public string Name { get; set; } = null!;

        public enum AccessoriesStatus
        {
            Inactive = 0,
            Active = 1
        }

        // 1 - N Products
        public ICollection<Product> Products { get; set; } = new List<Product>();

        public static Accessories create(string Name) => new Accessories
        {
            Name = Name,
        };
    }
}
