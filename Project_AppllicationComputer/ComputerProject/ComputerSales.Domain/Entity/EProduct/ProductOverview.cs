namespace ComputerSales.Domain.Entity.EProduct
{
    public class ProductOverview
    {
        protected ProductOverview() { }
        public int ProductOverviewId { get; private set; }
        public long ProductId { get; private set; }
        public string TextContent { get; private set; } = "";
        public DateTime CreateDate { get; private set; }

        //1 Product sẽ có 1 OverView ( Despcrition dài) cho Product đó : 1-1 Product (0-1)
        public Product? Product { get; private set; }

        public static ProductOverview Create(long ProductId, string TextContent)
            => new ProductOverview
            {
                ProductId = ProductId,
                TextContent = TextContent,
            };

        public string UpdateText(string TextContent)
        {
            return this.TextContent = TextContent;
        }
    }
}
