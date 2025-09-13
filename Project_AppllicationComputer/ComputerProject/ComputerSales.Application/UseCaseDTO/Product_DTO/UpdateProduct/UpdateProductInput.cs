namespace ComputerSales.Application.UseCaseDTO.Product_DTO.UpdateProduct
{

    /// <summary>
    /// DTO cập nhật. RowVersion dùng Base64 để qua JSON.
    /// Chỉ các field cho phép sửa mới đưa vào đây.
    /// </summary>
    public sealed record UpdateProductInput(
            long ProductID,
            string ShortDescription,
            int Status,
            long AccessoriesID,
            long ProviderID,
            string Slug,
            string SKU,
            string RowVersionBase64   // bắt buộc
        );
}
