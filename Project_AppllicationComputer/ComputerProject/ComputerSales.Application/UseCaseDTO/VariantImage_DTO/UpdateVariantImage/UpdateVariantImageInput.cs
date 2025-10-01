namespace ComputerSales.Application.UseCaseDTO.VariantImage.UpdateVariantImage
{
    public sealed record UpdateVariantImageInput(
          int VariantId,
         string Url,
         int SortOrder,
         string DescriptionImg
        );
}
