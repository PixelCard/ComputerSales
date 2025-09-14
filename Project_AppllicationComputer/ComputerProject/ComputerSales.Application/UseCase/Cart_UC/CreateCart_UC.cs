using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.Cart_DTO;
using ComputerSales.Domain.Entity.ECart;

namespace ComputerSales.Application.UseCase.Cart_UC
{
    public class CreateCart_UC
    {
        private readonly IUnitOfWorkApplication _uow;
        private readonly IRespository<Cart> _repo;

        public CreateCart_UC(IUnitOfWorkApplication uow, IRespository<Cart> repo)
        {
            _uow = uow;
            _repo = repo;
        }

        public async Task<CartOutputDTO> HandleAsync(CartInputDTO input, CancellationToken ct = default)
        {
            // Dùng factory method để tạo entity
            var entity = Cart.Create(
                input.UserID,
                input.Subtotal,
                input.DiscountTotal,
                input.ShippingFee,
                input.Status
            );

            entity.ExpiresAT = input.ExpiresAT;

            await _repo.AddAsync(entity, ct);
            await _uow.SaveChangesAsync(ct);

            return entity.ToResult();
        }
    }
}
