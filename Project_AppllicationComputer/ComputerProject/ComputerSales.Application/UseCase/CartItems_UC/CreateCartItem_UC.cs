using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.CartItem_DTO;
using ComputerSales.Domain.Entity.ECart;

public class CreateCartItem_UC
{
    private readonly IUnitOfWorkApplication _uow;
    private readonly IRespository<CartItem> _repo;

    public CreateCartItem_UC(IUnitOfWorkApplication uow, IRespository<CartItem> repo)
    {
        _uow = uow;
        _repo = repo;
    }

    public async Task<CartItemOutputDTO> HandleAsync(CartItemInputDTO input, CancellationToken ct = default)
    {
        var entity = input.ToEntity();
        await _repo.AddAsync(entity, ct);
        await _uow.SaveChangesAsync(ct);

        return entity.ToResult();
    }
}
