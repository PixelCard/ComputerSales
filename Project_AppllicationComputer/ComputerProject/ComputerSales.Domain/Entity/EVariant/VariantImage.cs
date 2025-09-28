using ComputerSales.Domain.Entity.EProduct;

namespace ComputerSales.Domain.Entity.EVariant
{
    public class VariantImage
    {
        public int Id { get; set; } //id của VariantImage
        public int VariantId { get; set; }
        public string Url { get; set; }
        public int SortOrder { get; set; }
        public string DescriptionImg { get; set; }

        public ProductVariant Variant { get; set; }
        public static VariantImage Create(int variantId, string url, int sortOrder, string descriptionImg)
        {
            return new VariantImage
            {
                VariantId = variantId,
                Url = url,
                SortOrder = sortOrder,
                DescriptionImg = descriptionImg
            };
        }
    }
}
