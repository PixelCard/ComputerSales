using ComputerSales.Domain.Entity.EVariant;

namespace ComputerSales.Application.UseCaseDTO.VariantPrice_DTO.VariantPriceInput_Output
{
    public sealed record VariantPriceOutputDTO(
            int Id,
        int VariantId,
         string Currency,
         decimal Price,
         decimal DiscountPrice,
         PriceStatus Status,
         DateTime? ValidFrom,
         DateTime? ValidTo);
}
