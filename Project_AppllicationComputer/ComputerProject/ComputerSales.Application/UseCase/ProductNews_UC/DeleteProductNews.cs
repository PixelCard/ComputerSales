using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.ProductNews_DTO;
using ComputerSales.Domain.Entity.ENews;

namespace ComputerSales.Application.UseCase.ProductNews_UC
{
    public class DeleteProductNews_UC
    {
        private readonly IUnitOfWorkApplication _uow;
        private readonly IRespository<ProductNews> _repo;

        public DeleteProductNews_UC(IUnitOfWorkApplication uow, IRespository<ProductNews> repo)
        {
            _uow = uow;
            _repo = repo;
        }

        public async Task<ProductNewsOutputDTO?> HandleAsync(int id, CancellationToken ct = default)
        {
            var entity = await _repo.GetByIdAsync(id, ct);
            if (entity == null) return null;

            await _repo.DeleteByIdAsync(entity.ProductNewsID, ct);
            await _uow.SaveChangesAsync(ct);

            return entity.ToResult();
        }

    }
}
