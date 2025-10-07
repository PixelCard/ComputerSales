using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.UseCaseDTO.Provider_DTO;
using ComputerSales.Domain.Entity.EProvider;

namespace ComputerSales.Application.UseCase.Provider_UC
{
    public class GetAllProviders_UC
    {
        private readonly IRespository<Provider> _repo;

        public GetAllProviders_UC(IRespository<Provider> repo)
        {
            _repo = repo;
        }

        public async Task<List<ProviderOutput>> HandleAsync(CancellationToken ct = default)
        {
            var list = await _repo.ListAsync(ct: ct);
            return list.Select(p => p.ToResult()).ToList();
        }
    }
}


