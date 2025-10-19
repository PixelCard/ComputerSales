using ComputerSales.Application.Interface.Cart_Interface;
using ComputerSales.Application.Interface.Interface_OrderFromCart;
using ComputerSales.Application.Interface.InterfaceCustomerRespo;
using ComputerSales.Application.UseCase.Product_UC;
using ComputerSales.Application.UseCaseDTO.Order_DTO;
using ComputerSales.Domain.Entity.E_Order;
using ComputerSales.Infrastructure.Persistence;
using ComputerSales.Application.UseCaseDTO.Product_DTO.GetByID;
using Microsoft.EntityFrameworkCore;
using ComputerSales.Application.UseCase.VariantPrice_UC.variantGetPriceByVariantID;
using ComputerSales.Application.UseCaseDTO.VariantPrice_DTO;
using ComputerSales.Domain.Entity.EProduct;
using ComputerSales.Domain.Entity.EPaymentVNPAYTransaction;

namespace ComputerSales.Infrastructure.Repositories.OrderCart_Respo
{
    public class OrderFromCartRespository : IOrderFromCart
    {
        private AppDbContext _db;

        private ICartReadRespository cartReadRespository;

        private ICustomerRespo customerRespo;

        private GetProduct_UC getProduct_UC;

        private variantGetPriceByVariantID_UC  variantGetPriceByVariantID_UC;

        public OrderFromCartRespository(
            AppDbContext db, 
            ICartReadRespository cartReadRespository, 
            ICustomerRespo customerRespo,
            GetProduct_UC getProduct_UC,
            variantGetPriceByVariantID_UC variantGetPriceByVariantID_UC)
        {
            _db = db;
            this.cartReadRespository = cartReadRespository;
            this.getProduct_UC= getProduct_UC;
            this.variantGetPriceByVariantID_UC= variantGetPriceByVariantID_UC;
            this.customerRespo = customerRespo;
        }

        public async Task<int> CreateFromCartAsync(
            int userId,
            string fullName, string phone, string? email, string address, string? notes,
            PaymentKind payment,
            CancellationToken ct)
        {
            // 1) Lấy cart
            var cart = await cartReadRespository.GetByUserAsync(userId, ct);

            // 2) Map payment → PaymentID
            var code = payment switch
            {
                PaymentKind.COD => "COD",
                PaymentKind.VNPAY => "VNPAY",
                _ => "COD"
            };

            var paymentId = await _db.PaymentMethods
                .Where(x => x.Code == code && x.IsActive)
                .Select(x => (int?)x.PaymentID)
                .FirstOrDefaultAsync(ct);

            if (paymentId is null)
                throw new InvalidOperationException($"Payment method '{code}' is not available.");

            // 3) Lấy customer
            var customer = await customerRespo.GetCustomerByUserID(userId, ct);

            if (customer is null) throw new InvalidOperationException("Customer not found");

            // 4) Tính lại subtotal/discount từ Variant Price chuẩn công thức của bạn
            decimal subtotal = 0m;
            decimal discountTotal = 0m;

            foreach (var it in cart.Items)
            {
                var req = new VariantGetPriceByVariantID_Input(it.ProductVariantID!.Value);
                var vp = await variantGetPriceByVariantID_UC.HandleAysnc(req, ct); 

                var unitBase = vp.unitPrice;                                       
                var unitFinal = vp.DiscountPrice > 0 ? vp.DiscountPrice : unitBase;  
                if (unitFinal > unitBase) unitFinal = unitBase;

                subtotal += unitBase * it.Quantity;               
                discountTotal += unitFinal * it.Quantity;           
            }

            var shipping = cart.ShippingFee; // hoặc tự tính

            // 5) Tạo Order
            var order = Order.Create(
                orderTime: DateTime.UtcNow,
                customerID: customer.CustomerID,
                paymentID: paymentId,
                orderStatus: OrderStatus.ChoXacNhan,
                OrderNote: notes,
                subtotal: subtotal,
                discountTotal: discountTotal,
                shippingFee: shipping
            );

            await using var tx = await _db.Database.BeginTransactionAsync(ct);

            _db.Orders.Add(order);
            await _db.SaveChangesAsync(ct); // cần OrderID

            // 6) Thêm OrderDetails 
            var details = new List<OrderDetail>(cart.Items.Count);
            foreach (var it in cart.Items)
            {
                var req = new VariantGetPriceByVariantID_Input(it.ProductVariantID!.Value);
                var vp = await variantGetPriceByVariantID_UC.HandleAysnc(req, ct);

                var unitBase = vp.unitPrice;
                var unitFinal = vp.DiscountPrice > 0 ? vp.DiscountPrice : unitBase;
                if (unitFinal > unitBase) unitFinal = unitBase;


                var d = OrderDetail.Create(
                    orderId: order.OrderID,
                    productId: it.ProductID,
                    productVariantId: it.ProductVariantID.Value,
                    quantity: it.Quantity,
                    unitPrice: unitBase,      //giá gốc           
                    discount: unitFinal,      // giá giảm trên 1 sản phẩm = variant discount value 
                    sku: it.SKU ?? string.Empty,
                    name: it.Name ?? string.Empty,
                    optionSummary: it.OptionSummary ?? string.Empty,
                    imageUrl: it.ImageUrl ?? string.Empty
                );

                details.Add(d);
            }

            _db.OrderDetails.AddRange(details);

            await _db.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);

            // 7) Xoá giỏ bằng repo, đừng Remove trong foreach
            await cartReadRespository.ClearAsync(userId, ct);

            return order.OrderID;
        }

        public async Task MarkPaidAsync(int orderId, PaymentKind payment, string transactionId, string? responseCode, CancellationToken ct)
        {
            // map enum -> code trong bảng PaymentMethods
            var code = payment switch
            {
                PaymentKind.VNPAY => "VNPAY",
                PaymentKind.COD => "COD",
                _ => "COD"
            };

            var paymentId = await _db.PaymentMethods
                .Where(x => x.Code == code && x.IsActive)
                .Select(x => (int?)x.PaymentID)
                .FirstOrDefaultAsync(ct)
                ?? throw new InvalidOperationException($"Payment method '{code}' is not available.");

            var order = await _db.Orders.FirstOrDefaultAsync(x => x.OrderID == orderId, ct)
                        ?? throw new InvalidOperationException($"Order #{orderId} not found");

            // Idempotent: nếu đã gắn VNPAY và trạng thái > Chờ xác nhận thì bỏ qua
            if (order.PaymentID == paymentId && order.OrderStatus > OrderStatus.ChoXacNhan)
                return;

            order.PaymentID = paymentId;
            order.OrderStatus = OrderStatus.DangDongGoi;

            await _db.SaveChangesAsync(ct);
        }
    }
}
