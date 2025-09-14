using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.CartItem_DTO.DeleteCartItem;
using ComputerSales.Domain.Entity.ECart;

public class DeleteCartItem_UC
{
    private readonly IUnitOfWorkApplication _uow;
    private readonly IRespository<CartItem> _repo;

    public DeleteCartItem_UC(IUnitOfWorkApplication uow, IRespository<CartItem> repo)
    {
        _uow = uow;
        _repo = repo;
    }

    public async Task<bool> HandleAsync(InputDeleteCartItem input, CancellationToken ct = default)
    {
        var entity = await _repo.GetByIdAsync(input.ID, ct);
        if (entity == null || entity.CartID != input.CartID) return false;

        await _repo.DeleteByIdAsync(input.ID, ct);
        await _uow.SaveChangesAsync(ct);

        return true;
    }
}
