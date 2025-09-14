using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.OrderDetail_DTO;
using ComputerSales.Domain.Entity.E_Order;

public class CreateOrderDetail_UC
{
    private readonly IUnitOfWorkApplication _uow;
    private readonly IRespository<OrderDetail> _repo;

    public CreateOrderDetail_UC(IUnitOfWorkApplication uow, IRespository<OrderDetail> repo)
    {
        _uow = uow;
        _repo = repo;
    }

    public async Task<OrderDetailOutputDTO> HandleAsync(int orderId, OrderDetailInputDTO input, CancellationToken ct = default)
    {
        var entity = input.ToEntity(orderId);
        await _repo.AddAsync(entity, ct);
        await _uow.SaveChangesAsync(ct);

        return entity.ToResult();
    }
}
