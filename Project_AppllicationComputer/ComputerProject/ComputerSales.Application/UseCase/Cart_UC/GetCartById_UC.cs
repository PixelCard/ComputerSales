using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.UseCaseDTO.Cart_DTO.GetCartById;
using ComputerSales.Application.UseCaseDTO.Cart_DTO;
using ComputerSales.Domain.Entity.ECart;

namespace ComputerSales.Application.UseCase.Cart_UC
{
    public class GetCartById_UC
    {
        private readonly IRespository<Cart> _repo;

        public GetCartById_UC(IRespository<Cart> repo)
        {
            _repo = repo;
        }

        public async Task<CartOutputDTO?> HandleAsync(InputGetCartByID input, CancellationToken ct = default)
        {
            var entity = await _repo.GetByIdAsync(input.ID, ct);
            if (entity == null || entity.UserID != input.UserID) return null;

            return entity.ToResult();
        }
    }
}
