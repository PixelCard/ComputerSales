using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.ProductNews_DTO;
using ComputerSales.Domain.Entity.ENews;

namespace ComputerSales.Application.UseCase.ProductNews_UC
{
    public class GetByIdProductNews_UC
    {
        private readonly IRespository<ProductNews> _repo;
        private readonly IUnitOfWorkApplication _uow;

        public GetByIdProductNews_UC(IRespository<ProductNews> repo, IUnitOfWorkApplication uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<ProductNewsOutputDTO?> HandleAsync(int id, CancellationToken ct = default)
        {
            var entity = await _repo.GetByIdAsync(id, ct);
            if (entity == null) return null;

            return entity.ToResult();
        }
    }
}
