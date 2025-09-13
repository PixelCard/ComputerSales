namespace ComputerSales.Application.UseCaseDTO.OptionalValue_DTO.UpdateOptionalValue_DTO
{
    public sealed record UpdateOptionalValueInput(
        int id,
        int OptionTypeId,
         string Value,
         int SortOrder);
}
