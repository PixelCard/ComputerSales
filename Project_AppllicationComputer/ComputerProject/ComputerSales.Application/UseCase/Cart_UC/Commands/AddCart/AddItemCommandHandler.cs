using ComputerSales.Application.Interface.Cart_Interface;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Domain.Entity.ECart;
using ComputerSales.Domain.Entity.EProduct;
using ComputerSales.Domain.Entity.EVariant;

namespace ComputerSales.Application.UseCase.Cart_UC.Commands.AddCart
{
    public sealed class AddItemCommandHandler
    {
        private readonly ICartReadRespository _read;
        private readonly ICartWriteRepository _write;
        const int LIMIT = 3;
        private readonly IUnitOfWorkApplication unitOfWorkApplication;

        public AddItemCommandHandler(ICartReadRespository read, 
            ICartWriteRepository _write, 
            IUnitOfWorkApplication _unitOfWorkApplication)
        { _read = read; this._write = _write; unitOfWorkApplication = _unitOfWorkApplication; }

        public async Task Handle(AddItemCommand cmd, CancellationToken ct = default)
        {
            if (cmd.Quantity <= 0) throw new ArgumentOutOfRangeException(nameof(cmd.Quantity));

            // 1) Lấy (hoặc tạo) giỏ của user
            var cart = await _read.GetByUserAsync(cmd.UserId, ct);

            if (cart is null)
            {
                cart = Cart.Create(cmd.UserId, subtotal: 0m, discountTotal: 0m, shippingFee: 0m); 

                cart = await _write.CreateAsync(cart, ct);
            }

            // 2) Ghép/cộng dồn nếu item đã tồn tại (product + variant trùng)
            var existed = cart.Items.FirstOrDefault(i =>
                i.ProductID == cmd.ProductId && i.ProductVariantID == cmd.ProductVariantId);

            if (existed is null)
            {
                if (cmd.ProductVariantId is not int vid)
                    throw new NotSupportedException("Add to cart without variant is not supported yet.");

                // Load variant (bao gồm nav: prices/images/options)
                var dict = await _read.GetVariantsAsync(new[] { vid }, ct);  

                if (!dict.TryGetValue(vid, out var v))
                    throw new InvalidOperationException($"Variant {vid} not found");

                // ---- CHỐT GIÁ từ VariantPrices ----
                var now = DateTime.UtcNow;

                var priceRow = v.VariantPrices?
                    .Where(p => p.Status == PriceStatus.Active
                                && (p.ValidFrom == null || p.ValidFrom <= now)
                                && (p.ValidTo == null || p.ValidTo >= now))
                    .OrderByDescending(p => p.ValidFrom  ?? DateTime.MinValue)
                    .FirstOrDefault();

                var unitPrice = Math.Max(0, priceRow.Price - (priceRow.DiscountPrice > 0 ? priceRow.DiscountPrice : 0));

                // ---- Ảnh đại diện từ VariantImages (theo SortOrder nhỏ nhất) ----
                var imageUrl = v.VariantImages?
                    .OrderBy(img => img.SortOrder)
                    .FirstOrDefault()?.Url ?? string.Empty;                          

                // ---- Option summary (fallback ID nếu thiếu tên) ----
                var optionSummary = (v.VariantOptionValues ?? Enumerable.Empty<VariantOptionValue>())
                    .Select(ov => ov.OptionalValue?.ToString() ?? ov.OptionalValueId.ToString())
                    .ToArray();                                                       
                var summaryText = optionSummary.Length > 0 ? string.Join(" / ", optionSummary) : "";

                // ---- Tên & SKU ----
                var sku = v.SKU;                                                    
                var name = v.Product?.ShortDescription ?? sku;                      

                // Tạo CartItem (snapshot)
                var item = new CartItem
                {
                    CartID = cart.ID,
                    ProductID = (int?)v.ProductId,
                    ProductVariantID = vid,
                    Quantity = cmd.Quantity,
                    UnitPrice = unitPrice,
                    SKU = sku,
                    Name = name,
                    OptionSummary = summaryText,
                    ImageUrl = imageUrl,
                    CreatedAt = now,
                    IsSelected = true
                };
                item.Quantity = Math.Min(cmd.Quantity, LIMIT);
                cart.Items.Add(item);
            }
            else
            {
                existed.Quantity = Math.Min(existed.Quantity + cmd.Quantity, LIMIT);
            }

            // 3) Tính lại tiền
            cart.Subtotal = cart.Items.Sum(i => i.UnitPrice * i.Quantity);
            cart.RecalculateTotals();

            // 4) Lưu DB (gọi SaveChanges ở tầng infra cho Cart/Items)
            unitOfWorkApplication.SaveChangesAsync(ct);
        }
    }
}


