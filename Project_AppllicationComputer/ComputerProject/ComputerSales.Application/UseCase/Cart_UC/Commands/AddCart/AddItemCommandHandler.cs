using ComputerSales.Application.Interface.Cart_Interface;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCase.Product_UC;
using ComputerSales.Application.UseCaseDTO.Product_DTO.GetByID;
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
        private readonly GetProduct_UC getProduct_UC;

        public AddItemCommandHandler(ICartReadRespository read, 
            ICartWriteRepository _write, 
            IUnitOfWorkApplication _unitOfWorkApplication,
            GetProduct_UC getProduct_UC)
        { 
            _read = read; 
            this._write = _write; 
            unitOfWorkApplication = _unitOfWorkApplication; 
            this.getProduct_UC = getProduct_UC; 
        }

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

                // Load variant 
                var dict = await _read.GetVariantsAsync(new[] { vid }, ct);

                if (!dict.TryGetValue(vid, out var v))
                    throw new InvalidOperationException($"Variant {vid} not found");

                var product = await getProduct_UC.HandleAsync(new ProductGetByIDInput(v.ProductId),ct);

                // ---- CHỐT GIÁ từ VariantPrices ----
                var now = DateTime.UtcNow;

                var priceRow = v.VariantPrices?
                    .Where(p => p.Status == PriceStatus.Active
                                && (p.ValidFrom == null || p.ValidFrom <= now)
                                && (p.ValidTo == null || p.ValidTo >= now))
                    .OrderByDescending(p => p.ValidFrom ?? DateTime.MinValue)
                    .FirstOrDefault();

                var unitPrice = Math.Max(0, priceRow.Price);

                // ---- Ảnh đại diện ----
                var imageUrl = v.VariantImages?
                    .OrderBy(img => img.SortOrder)
                    .FirstOrDefault()?.Url ?? string.Empty;

                // ---- Option summary ----
                var optionNames = (v.VariantOptionValues ?? Enumerable.Empty<VariantOptionValue>())
                    .OrderBy(ov => ov.OptionalValue?.SortOrder ?? 0)
                    .Select(ov =>
                        // Ưu tiên tên giá trị thật (vd: "QHD", "WQHD"), fallback về Id nếu null
                        !string.IsNullOrWhiteSpace(ov.OptionalValue?.Value)
                            ? ov.OptionalValue!.Value
                            : ov.OptionalValueId.ToString()
                    )
                    .ToList();

                var optionSummaryText = optionNames.Count > 0 ? string.Join(" / ", optionNames) : "";

                // ---- Tên & SKU ----
                var sku = v.SKU;


                // Base name: ưu tiên Slug để redirect đúng; nếu rỗng thì dùng ShortDescription; cuối cùng fallback SKU
                var baseName = !string.IsNullOrWhiteSpace(product?.Slug)
                                    ? product!.Slug
                                    : (!string.IsNullOrWhiteSpace(product?.ShortDescription)
                                        ? product!.ShortDescription
                                        : sku);

                // Name hiển thị trong cart = "Slug  - OptionalValues"
                var name = string.IsNullOrEmpty(optionSummaryText)
                            ? baseName
                            : $"{baseName} - {optionSummaryText}";

                // Tạo CartItem (snapshot)
                var item = new CartItem
                {
                    CartID = cart.ID,
                    ProductID = (int?)v.ProductId,
                    ProductVariantID = vid,
                    Quantity = Math.Min(cmd.Quantity, LIMIT),
                    UnitPrice = unitPrice,
                    SKU = sku,
                    Name = name,
                    OptionSummary = optionSummaryText,
                    ImageUrl = imageUrl,
                    CreatedAt = now,
                    IsSelected = true
                };

                if (cmd.OptionalValueId is int ovId)
                {
                    var ov = await _read.GetOptionalValueAsync(ovId, ct);
                    if (ov != null)
                    {
                        item.UnitPrice += (ov.Price);
                        var optSummary = $"{ov.OptionType?.Name}: {ov.Value}";
                        item.OptionSummary = string.IsNullOrWhiteSpace(item.OptionSummary)
                            ? optSummary
                            : $"{item.OptionSummary} / {optSummary}";
                    }
                }
                cart.Items.Add(item);
            }
            else
            {
                existed.Quantity = Math.Min(existed.Quantity + cmd.Quantity, LIMIT);
            }

            // 3) Tính lại tiền hàng (Subtotal)
            cart.Subtotal = cart.Items.Sum(i => i.UnitPrice * i.Quantity);

            // 3.1) TÍNH LẠI DISCOUNT TOTAL từ VariantPrices đang active
            {
                var now = DateTime.UtcNow;

                var variantIds = cart.Items
                    .Where(i => i.ProductVariantID.HasValue)
                    .Select(i => i.ProductVariantID!.Value)
                    .Distinct()
                    .ToArray();

                decimal discountTotal = 0m;

                if (variantIds.Length > 0)
                {
                    var variantsMap = await _read.GetVariantsAsync(variantIds, ct);

                    foreach (var ci in cart.Items)
                    {
                        if (ci.ProductVariantID is not int pid) continue;

                        if (!variantsMap.TryGetValue(pid, out var vv) || vv.VariantPrices == null) continue;

                        var pr = vv.VariantPrices
                            .Where(p => p.Status == PriceStatus.Active
                                        && (p.ValidFrom == null || p.ValidFrom <= now)
                                        && (p.ValidTo == null || p.ValidTo >= now))
                            .OrderByDescending(p => p.ValidFrom ?? DateTime.MinValue)
                            .FirstOrDefault();

                        if (pr is null) continue;

                        // Mặc định: DiscountPrice = số tiền giảm trên mỗi đơn vị
                        var discountPerUnit = pr.DiscountPrice > 0 ? pr.DiscountPrice : 0m;

                        discountTotal += discountPerUnit * ci.Quantity;
                    }
                }

                cart.DiscountTotal = Math.Max(0, discountTotal);
            }

            // 3.2) Tổng cuối cùng
            cart.RecalculateTotals();

            // 4) Lưu DB
            unitOfWorkApplication.SaveChangesAsync(ct);
        }
    }
}


