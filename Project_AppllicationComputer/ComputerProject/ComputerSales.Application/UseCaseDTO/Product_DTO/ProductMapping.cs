using ComputerSales.Application.UseCaseDTO.Product_DTO.UpdateProduct;
using ComputerSales.Domain.Entity.EProduct;

namespace ComputerSales.Application.UseCaseDTO.Product_DTO
{
    public static class ProductMapping
    {
        public static Product ToEnity(this ProductDTOInput input)
        {
            return Product.Create(
                input.AccessoriesID,
                input.ProviderID,
                input.ShortDescription,
                input.SKU,
                input.Slug
            );
        }



        public static ProductOutputDTOcs ToResult(this Product e)
        {
            return new ProductOutputDTOcs(e.ProductID,
            e.ShortDescription,
            (int)e.Status,
            e.AccessoriesID,
            e.ProviderID,
            e.Slug,
            e.SKU,
            Convert.ToBase64String(e.RowVersion ?? Array.Empty<byte>())
            );
        }

        public static void ApplyUpdate(this Product e, UpdateProductInput dto)
        {
            e.ShortDescription = dto.ShortDescription;
            e.Status = (ProductStatus)dto.Status;
            e.AccessoriesID = dto.AccessoriesID;
            e.ProviderID = dto.ProviderID;
            e.Slug = dto.Slug;
            e.SKU = dto.SKU;
        }
    }
}
