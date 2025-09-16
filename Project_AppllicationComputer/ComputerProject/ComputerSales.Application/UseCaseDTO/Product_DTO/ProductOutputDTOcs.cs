namespace ComputerSales.Application.UseCaseDTO.Product_DTO
{
    public sealed record ProductOutputDTOcs(
        long ProductID,
        string ShortDescription,
        int Status,
        long AccessoriesID,
        long ProviderID,
        string Slug,
        string SKU,
        string RowVersionBase64
    );

   
}
