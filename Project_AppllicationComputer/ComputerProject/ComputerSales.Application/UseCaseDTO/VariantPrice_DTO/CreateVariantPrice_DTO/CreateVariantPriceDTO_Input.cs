using ComputerSales.Domain.Entity.EVariant;

namespace ComputerSales.Application.UseCaseDTO.VariantPrice_DTO.CreateVariantPrice_DTO
{
    public sealed record CreateVariantPriceDTO_Input(
         int VariantId,
         string Currency,
         decimal Price,
         decimal DiscountPrice,
         PriceStatus Status,
         DateTime? ValidFrom,
         DateTime? ValidTo );
}
