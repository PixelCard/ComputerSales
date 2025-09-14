using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.CartItem_DTO.UpdateCartItem;
using ComputerSales.Application.UseCaseDTO.CartItem_DTO;
using ComputerSales.Domain.Entity.ECart;

public class UpdateCartItem_UC
{
    private readonly IUnitOfWorkApplication _uow;
    private readonly IRespository<CartItem> _repo;

    public UpdateCartItem_UC(IUnitOfWorkApplication uow, IRespository<CartItem> repo)
    {
        _uow = uow;
        _repo = repo;
    }

    public async Task<CartItemOutputDTO?> HandleAsync(InputUpdateCartItem input, CancellationToken ct = default)
    {
        var entity = await _repo.GetByIdAsync(input.ID, ct);
        if (entity == null) return null;

        entity.Quantity = input.Quantity;
        entity.IsSelected = input.IsSelected;

        await _uow.SaveChangesAsync(ct);

        return entity.ToResult();
    }
}
