using ComputerSales.Domain.Entity.EProvider;

namespace ComputerSales.Application.UseCaseDTO.Provider_DTO
{
    public static class ProviderMapping
    {
        public static Provider ToEnity(this ProviderInput input)
            => Provider.create(input.ProviderName);

        public static ProviderOutput ToResult(this Provider input)
            => new ProviderOutput(input.ProviderID, input.ProviderName);
    }
}
