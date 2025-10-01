namespace ComputerSales.Application.UseCaseDTO.VariantImage
{
    public sealed record VariantImageOutputDTO(
         int Id,
         int VariantId,
         string Url,
         int SortOrder,
         string DescriptionImg);


}
