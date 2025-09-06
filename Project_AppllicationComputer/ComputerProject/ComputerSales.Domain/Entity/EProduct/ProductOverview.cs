using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
