using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.Cart_DTO.DeleteCart;
using ComputerSales.Domain.Entity.ECart;

namespace ComputerSales.Application.UseCase.Cart_UC
{
    public class DeleteCart_UC
    {
        private readonly IUnitOfWorkApplication _uow;
        private readonly IRespository<Cart> _repo;

        public DeleteCart_UC(IUnitOfWorkApplication uow, IRespository<Cart> repo)
        {
            _uow = uow;
            _repo = repo;
        }

        public async Task<bool> HandleAsync(InputDeleteCart input, CancellationToken ct = default)
        {
            var entity = await _repo.GetByIdAsync(input.ID, ct);
            if (entity == null || entity.UserID != input.UserID) return false;

            await _repo.DeleteByIdAsync(input.ID, ct);
            await _uow.SaveChangesAsync(ct);

            return true;
        }
    }
}
