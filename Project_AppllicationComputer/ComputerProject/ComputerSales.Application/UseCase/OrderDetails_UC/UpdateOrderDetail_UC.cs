using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.OrderDetail_DTO;
using ComputerSales.Domain.Entity.E_Order;

public class UpdateOrderDetail_UC
{
    private readonly IUnitOfWorkApplication _uow;
    private readonly IRespository<OrderDetail> _repo;

    public UpdateOrderDetail_UC(IUnitOfWorkApplication uow, IRespository<OrderDetail> repo)
    {
        _uow = uow;
        _repo = repo;
    }

    public async Task<OrderDetailOutputDTO?> HandleAsync(OrderDetailOutputDTO input, CancellationToken ct = default)
    {
        var entity = await _repo.GetByIdAsync(input.OrderID, ct);
        if (entity == null) return null;

        entity.ProductID = input.ProductID;
        entity.ProductVariantID = input.ProductVariantID;
        entity.Quantity = input.Quantity;
        entity.UnitPrice = input.UnitPrice;
        entity.Discount = input.Discount;
        entity.SKU = input.SKU;
        entity.Name = input.Name;
        entity.OptionSummary = input.OptionSummary;
        entity.ImageUrl = input.ImageUrl;
        //entity.TotalPrice = (input.UnitPrice - input.Discount) * input.Quantity;

        await _uow.SaveChangesAsync(ct);

        return entity.ToResult();
    }
}
