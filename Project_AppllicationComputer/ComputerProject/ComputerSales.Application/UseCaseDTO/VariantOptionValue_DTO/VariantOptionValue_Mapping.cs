using ComputerSales.Domain.Entity.EVariant;

namespace ComputerSales.Application.UseCaseDTO.VariantOptionValue_DTO
{
    public static class VariantOptionValue_Mapping
    {
        public static VariantOptionValue ToEnity(this VariantOptionValueInput_DTO input)
        {
            return VariantOptionValue.create(
                 input.VariantId,
                 input.OptionalValueId
            );
        }
        public static VariantOptionValueOutput_DTO ToResult(this VariantOptionValue input)
        {
            return new VariantOptionValueOutput_DTO(
                 input.VariantId,
                 input.OptionalValueId
             );
        }
    }
}
