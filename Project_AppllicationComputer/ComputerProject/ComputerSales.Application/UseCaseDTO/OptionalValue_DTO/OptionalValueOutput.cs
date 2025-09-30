namespace ComputerSales.Application.UseCaseDTO.OptionalValue_DTO
{
    public sealed record OptionalValueOutput(
         int Id,
         int OptionTypeId,
         string Value,
         int SortOrder,
         decimal Price);
}
