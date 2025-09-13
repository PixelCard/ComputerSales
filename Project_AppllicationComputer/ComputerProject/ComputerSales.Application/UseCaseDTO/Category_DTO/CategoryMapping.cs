using ComputerSales.Domain.Entity.ECategory;

namespace ComputerSales.Application.UseCaseDTO.Category_DTO
{
    public static class CategoryMapping
    {
        public static Accessories ToEntity(this CategoryInput input)
            => Accessories.create(input.name);

        public static CategoryOutput ToResult(this Accessories input)
            => new CategoryOutput(input.AccessoriesID, input.Name);
    }
}
