using ComputerSales.Domain.Entity.EOptional;

namespace ComputerSales.Application.UseCaseDTO.OptionalType_DTO
{
    public static class OptionalTypeMapping
    {
        public static OptionType ToEnity(this OptionalTypeInput input)
            => OptionType.create(input.Code, input.Name);   

        public static OptionalTypeOutput ToResult(this OptionType input)
            => new OptionalTypeOutput(input.Id,input.Code, input.Name);
    }
}
