using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.Cart_DTO.UpdateCart;
using ComputerSales.Application.UseCaseDTO.Cart_DTO;
using ComputerSales.Domain.Entity.ECart;

namespace ComputerSales.Application.UseCase.Cart_UC
{
    public class UpdateCart_UC
    {
        private readonly IUnitOfWorkApplication _uow;
        private readonly IRespository<Cart> _repo;

        public UpdateCart_UC(IUnitOfWorkApplication uow, IRespository<Cart> repo)
        {
            _uow = uow;
            _repo = repo;
        }

        public async Task<CartOutputDTO?> HandleAsync(InputUpdateCart input, CancellationToken ct = default)
        {
            var entity = await _repo.GetByIdAsync(input.IDCart, ct);
            if (entity == null || entity.UserID != input.UserID) return null;

            entity.Status = input.Status;
            entity.Subtotal = input.Subtotal;
            entity.DiscountTotal = input.DiscountTotal;
            entity.ShippingFee = input.ShippingFee;
            entity.ExpiresAT = input.ExpiresAT;
            entity.RecalculateTotals();

            await _uow.SaveChangesAsync(ct);

            return entity.ToResult();
        }
    }
}
