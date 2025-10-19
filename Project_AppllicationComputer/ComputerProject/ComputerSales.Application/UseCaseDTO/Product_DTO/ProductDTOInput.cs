namespace ComputerSales.Application.UseCaseDTO.Product_DTO
{
    public sealed record ProductDTOInput(
        string ShortDescription,
        int Status,
        long AccessoriesID,
        long ProviderID,
        string Slug,
        string? SKU

    );


}
