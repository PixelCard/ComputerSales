using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.UseCaseDTO.VariantOptionValue_DTO;
using ComputerSales.Application.UseCaseDTO.VariantOptionValue_DTO.getVariantOptionValue;
using ComputerSales.Domain.Entity.EVariant;

namespace ComputerSales.Application.UseCase.VariantOptionValue_UC
{
    public class GetByIdVariantOptionValue_UC
    {
        private readonly IRespository<VariantOptionValue> _respository;

        public GetByIdVariantOptionValue_UC(IRespository<VariantOptionValue> respository)
        {
            _respository = respository;
        }

        public async Task<VariantOptionValueOutput_DTO?> HandleAsync(getVariantOptionValueByID_Input input, CancellationToken ct)
        {
            var entity = await _respository.GetByIdAsync((input.VariantId, input.OptionalValueId), ct);
      
            if (entity == null) return null;
            return entity.ToResult();
        }
    }
}
