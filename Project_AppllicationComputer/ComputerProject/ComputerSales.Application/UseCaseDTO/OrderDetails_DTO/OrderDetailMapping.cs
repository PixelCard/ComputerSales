using ComputerSales.Application.UseCaseDTO.OrderDetail_DTO;
using ComputerSales.Domain.Entity.E_Order;

public static class OrderDetailMapping
{
    public static OrderDetail ToEntity(this OrderDetailInputDTO input, int orderId)
    {
        return OrderDetail.Create(
            orderId,
            input.ProductID,
            input.ProductVariantID,
            input.Quantity,
            input.UnitPrice,
            input.Discount,
            input.SKU,
            input.Name,
            input.OptionSummary,
            input.ImageUrl
        );
    }

    public static OrderDetailOutputDTO ToResult(this OrderDetail e)
    {
        return new OrderDetailOutputDTO(
            e.OrderID,
            e.ProductID,
            e.ProductVariantID,
            e.Quantity,
            e.UnitPrice,
            e.Discount,
            e.TotalPrice,
            e.SKU,
            e.Name,
            e.OptionSummary,
            e.ImageUrl
        );
    }
}
