using ComputerSales.Application.Interface.Interface_VariantPriceRespo;
using ComputerSales.Application.UseCaseDTO.VariantPrice_DTO;

namespace ComputerSales.Application.UseCase.VariantPrice_UC.variantGetPriceByVariantID
{
    public class variantGetPriceByVariantID_UC
    {
        private readonly IVariantPriceRespo variantPriceRespo;

        public variantGetPriceByVariantID_UC(IVariantPriceRespo variantPriceRespo)
        {
            this.variantPriceRespo = variantPriceRespo;
        }

        public async Task<variantGetPriceByVariantID_Output?> HandleAysnc(VariantGetPriceByVariantID_Input input,CancellationToken ct)
        {
            var Entity = variantPriceRespo.variantGetPriceByVariantID(input.variantID,ct);

            var Result = new variantGetPriceByVariantID_Output(Entity.Result.Price, Entity.Result.DiscountPrice);

            return Result;
        }
    }
}
