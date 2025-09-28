using ComputerSales.Domain.Entity.EOptional;

namespace ComputerSales.Application.UseCaseDTO.OptionalValue_DTO
{
    public static class OptionalValueMapping
    {
        public static OptionalValue ToEnity(this OptionalValueInput input)
            => OptionalValue.create(input.OptionTypeId,input.Value,input.SortOrder, input.Price);

        public static OptionalValueOutput ToResult(this OptionalValue input)
            => new OptionalValueOutput(input.Id,input.OptionTypeId,input.Value,input.SortOrder, input.Price);
    }
}
