using ComputerSales.Application.UseCaseDTO.Role_DTO;
using ComputerSales.Application.UseCaseDTO.VariantPrice_DTO.VariantPriceInput_Output;
using ComputerSales.Domain.Entity.EAccount;
using ComputerSales.Domain.Entity.EVariant;
using System.ComponentModel.DataAnnotations;

namespace ComputerSales.Application.UseCaseDTO.VariantPrice_DTO
{
    public static class VariantPriceMapping
    {
        public static VariantPrice ToEnity(this VariantPriceInputDTO input)
        {
            return VariantPrice.Create(
                 input.VariantId,
                 input.Currency,
                 input.Price,
                 input.DiscountPrice,
                 input.Status,
                 input?.ValidFrom,
                 input?.ValidTo
            );
        }
        public static VariantPriceOutputDTO ToResult(this VariantPrice input)
        {
            return new VariantPriceOutputDTO(
                 input.Id,
                 input.VariantId,
                 input.Currency,
                 input.Price,
                 input.DiscountPrice,
                 input.Status,
                 input?.ValidFrom,
                 input?.ValidTo
             );
        }
    }
}
