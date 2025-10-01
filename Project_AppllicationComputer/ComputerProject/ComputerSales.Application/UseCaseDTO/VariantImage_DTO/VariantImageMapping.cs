using ComputerSales.Application.UseCaseDTO.NewFolder;
using ComputerSales.Application.UseCaseDTO.VariantImage;

namespace ComputerSales.Application.UseCaseDTO.VariantImageDTO
{
    public static class VariantImageMapping
    {
        public static Domain.Entity.EVariant.VariantImage ToEntity(this VariantImageInputDTO input)
        {
            return Domain.Entity.EVariant.VariantImage.Create(
                input.VariantId,
                input.Url,
                input.SortOrder,
                input.DescriptionImg
            );
        }

        public static VariantImageOutputDTO ToResult(this Domain.Entity.EVariant.VariantImage e)
        {
            return new VariantImageOutputDTO(
                e.Id,
                e.VariantId,
                e.Url,
                e.SortOrder,
                e.DescriptionImg
            );
        }
    }
}
