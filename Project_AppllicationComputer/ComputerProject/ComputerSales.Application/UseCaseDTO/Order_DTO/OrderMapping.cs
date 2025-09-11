using ComputerSales.Domain.Entity.E_Order;

namespace ComputerSales.Application.UseCaseDTO.Order_DTO
{
    public static class OrderMapping
    {
        public static Order ToEntity(this OrderInputDTO input)
        {
            var e = Order.Create(
                input.OrderTime,
                input.IDCustomer,
                input.PaymentID,
                input.OrderStatus,
                input.Subtotal,
                input.DiscountTotal,
                input.ShippingFee
            );
            e.Status = input.Status; // nhớ set
            return e;
        }

        public static OrderOutputDTO ToResult(this Order e)
        {
            return new OrderOutputDTO(
                e.OrderID,        // <- THÊM & đặt đúng vị trí đầu tiên
                e.OrderTime,
                e.IDCustomer,
                e.PaymentID,
                e.OrderStatus,
                e.GrandTotal,
                e.Status
            );
        }
    }
}
