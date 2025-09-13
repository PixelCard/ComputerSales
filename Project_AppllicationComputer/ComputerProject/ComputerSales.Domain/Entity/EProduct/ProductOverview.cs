namespace ComputerSales.Domain.Entity.EProduct
{
    public enum OverviewBlockType
    {
        Text,
        List,
        Image,
        Logo,
        Table
    }

    public class ProductOverview
    {
        protected ProductOverview() { }
        public int ProductOverviewId { get; private set; }
        public long ProductId { get; private set; }
        public OverviewBlockType BlockType { get; private set; }
        public string TextContent { get; private set; } = "";
        public string? ImageUrl { get; private set; }
        public string? Caption { get; private set; }
        public int DisplayOrder { get; private set; }
        public DateTime CreateDate { get; private set; }

        //1 Product sẽ có 1 OverView ( Despcrition dài) cho Product đó : 1-1 Product (0-1)
        public Product? Product { get; private set; }

        public static ProductOverview Create(long ProductId, OverviewBlockType BlockType, string TextContent,
            string? ImageUrl, string? Caption, int DisplayOrder)
            => new ProductOverview
            {
                ProductId = ProductId,
                BlockType = BlockType,
                TextContent = TextContent,
                ImageUrl = ImageUrl,
                Caption = Caption,
                DisplayOrder = DisplayOrder
            };

        public string UpdateText(string TextContent)
        {
            return this.TextContent = TextContent;
        }


        public string UpdateImageUrl(string ImageUrl) 
        { 
            return this.ImageUrl=ImageUrl;
        }

        public string UpdateCaption(string Caption)
        {
            return this.Caption = Caption;
        }
    }
}
