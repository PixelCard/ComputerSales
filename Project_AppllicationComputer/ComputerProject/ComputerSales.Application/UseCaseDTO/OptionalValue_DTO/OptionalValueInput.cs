namespace ComputerSales.Application.UseCaseDTO.OptionalValue_DTO
{
    public sealed record OptionalValueInput(         
        int OptionTypeId,
        string Value ,
        int SortOrder,
        decimal Price);
}
