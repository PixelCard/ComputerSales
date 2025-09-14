using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.UseCaseDTO.OrderDetail_DTO;
using ComputerSales.Domain.Entity.E_Order;

public class GetByIdOrderDetail_UC
{
    private readonly IRespository<OrderDetail> _repo;

    public GetByIdOrderDetail_UC(IRespository<OrderDetail> repo)
    {
        _repo = repo;
    }

    public async Task<OrderDetailOutputDTO?> HandleAsync(int id, CancellationToken ct = default)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        return entity?.ToResult();
    }
}
