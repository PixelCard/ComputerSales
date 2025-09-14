using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerSales.Domain.Entity.ENews
{
    public class ProductNews
    {
        [Column("ProductNewsID")]
        public int ProductNewsID { get; set; }

        public string BlockType { get; set; } = default!; // ENUM text, list, image, logo, table
        public string? TextContent { get; set; }
        public string? ImageUrl { get; set; }
        public string? Caption { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime CreateDate { get; set; }

        // Factory method để tạo nhanh entity
        public static ProductNews Create(
            string blockType,
            string? textContent,
            string? imageUrl,
            string? caption,
            int displayOrder,
            DateTime createDate)
        {
            return new ProductNews
            {
                BlockType = blockType,
                TextContent = textContent,
                ImageUrl = imageUrl,
                Caption = caption,
                DisplayOrder = displayOrder,
                CreateDate = createDate
            };
        }
    }
}
