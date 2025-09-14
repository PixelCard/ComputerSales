using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.UseCaseDTO.CartItem_DTO.GetCartItemById;
using ComputerSales.Application.UseCaseDTO.CartItem_DTO;
using ComputerSales.Domain.Entity.ECart;

public class GetCartItemById_UC
{
    private readonly IRespository<CartItem> _repo;

    public GetCartItemById_UC(IRespository<CartItem> repo)
    {
        _repo = repo;
    }

    public async Task<CartItemOutputDTO?> HandleAsync(InputGetCartItemByID input, CancellationToken ct = default)
    {
        var entity = await _repo.GetByIdAsync(input.ID, ct);
        if (entity == null || entity.CartID != input.CartID) return null;
        return entity.ToResult();
    }
}
