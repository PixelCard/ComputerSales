using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Domain.Entity.E_Order;

public class DeleteOrderDetail_UC
{
    private readonly IUnitOfWorkApplication _uow;
    private readonly IRespository<OrderDetail> _repo;

    public DeleteOrderDetail_UC(IUnitOfWorkApplication uow, IRespository<OrderDetail> repo)
    {
        _uow = uow;
        _repo = repo;
    }

    public async Task<bool> HandleAsync(int id, CancellationToken ct = default)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null) return false;

        await _repo.DeleteByIdAsync(id, ct);
        await _uow.SaveChangesAsync(ct);

        return true;
    }
}
