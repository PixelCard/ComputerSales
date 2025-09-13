namespace ComputerSales.Application.UseCaseDTO.Order_DTO.UpdateOrder
{
    public sealed record InputUpdateOrderByIdOrderCustomer(
      int OrderID,
      int CustomerID,
      int OrderStatus,
      decimal GrandTotal,
      bool Status
      );
}
