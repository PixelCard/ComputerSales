using ComputerSales.Application.Interface.Cart_Interface;
using ComputerSales.Application.UseCaseDTO.Cart_DTO.Cart_Items;
using ComputerSales.Application.UseCaseDTO.Cart_DTO.Cart_Page;
using ComputerSales.Domain.Entity.EProduct;
using ComputerSales.Domain.Entity.EVariant;

namespace ComputerSales.Application.UseCase.Cart_UC.Queries.GetCartPage
{
    public sealed class GetCartPageQueryHandler
    {
        private readonly ICartReadRespository _repo;
        const decimal DEFAULT_PLAN_PRICE = 24m;
        public GetCartPageQueryHandler(ICartReadRespository repo) { _repo = repo; }

        public async Task<CartPageDTO> Handle(GetCartPageQuery q, CancellationToken ct = default)
        {
            //1.Lấy giỏ hàng từ repo
            var cart = await _repo.GetByUserAsync(q.UserId, ct);
            if (cart == null) return new CartPageDTO();

            // 2) lấy variants cho item trong giỏ
            var variantIds = cart.Items.Where(i => i.ProductVariantID.HasValue)
                                       .Select(i => i.ProductVariantID!.Value)
                                       .Distinct()
                                       .ToArray();
            var variants = await _repo.GetVariantsAsync(variantIds, ct);

            // 2b) lấy trước variants của protection plans (nếu có)
            var productIdsMain = cart.Items.Where(i => !i.ParentItemID.HasValue && i.ProductID.HasValue)
                                           .Select(i => i.ProductID!.Value)
                                           .Distinct()
                                           .ToList();

            // 3) chọn giá đang hiệu lực
            var now = DateTime.UtcNow;

            VariantPrice? Active(IEnumerable<VariantPrice> ps) =>
                ps.Where(p => p.Status == PriceStatus.Active
                           && (p.ValidFrom == null || p.ValidFrom <= now)
                           && (p.ValidTo == null || p.ValidTo >= now))
                  .OrderByDescending(p => p.ValidFrom ?? DateTime.MinValue)
                  .FirstOrDefault();

            // 4) option summary
            string BuildOptionSummary(ProductVariant? v)
            {
                if (v?.VariantOptionValues == null || v.VariantOptionValues.Count == 0) return "";
                var parts = v.VariantOptionValues
                    .Where(x => x.OptionalValue != null && x.OptionalValue.OptionType != null)
                    .Select(x => new { T = x.OptionalValue!.OptionType!.Name, V = x.OptionalValue!.Value, O = x.OptionalValue!.SortOrder })
                    .OrderBy(x => x.O).GroupBy(x => x.T)
                    .Select(g => $"{g.Key}: {string.Join(", ", g.Select(x => x.V))}");
                return string.Join(" • ", parts);
            }

            // 5) map từng dòng
            var lines = new List<CartItemsDTO>();
            foreach (var i in cart.Items)
            {
                variants.TryGetValue(i.ProductVariantID ?? -1, out var v);
                var ap = Active(v?.VariantPrices ?? Array.Empty<VariantPrice>());
                var list = ap?.Price ?? i.UnitPrice;
                var sale = ap != null
                    ? Math.Max(0, ap.Price - (ap.DiscountPrice > 0 ? ap.DiscountPrice : 0))
                    : i.UnitPrice;

                var imageUrl = (v?.VariantImages != null && v.VariantImages.Count > 0)
                    ? v.VariantImages.OrderBy(vi => vi.SortOrder).First().Url
                    : i.ImageUrl;

                var line = new CartItemsDTO
                {
                    CartItemId = i.ID,
                    ImageUrl = imageUrl,
                    Name = i.Name,
                    SKU = i.SKU,
                    OptionSummary = string.IsNullOrWhiteSpace(i.OptionSummary) ? BuildOptionSummary(v) : i.OptionSummary,
                    Quantity = i.Quantity,
                    PerItemLimit = i.PerItemLimit,
                    ListPrice = list,
                    SalePrice = sale,
                    IsChildService = i.ParentItemID.HasValue,
                    ParentItemId = i.ParentItemID
                };
                lines.Add(line);
            }

            // 6) tổng: tính cả service để khớp grand total
            var subtotal = lines.Sum(l => l.SalePrice * l.Quantity);
            var itemsCount = lines.Count(l => !l.IsChildService);

            return new CartPageDTO
            {
                CartId = cart.ID,
                ItemsCount = itemsCount,
                Subtotal = subtotal,
                DiscountTotal = cart.DiscountTotal,
                ShippingFee = cart.ShippingFee,
                TaxTotal = 0m,
                GrandTotal = subtotal - cart.DiscountTotal + cart.ShippingFee,
                Lines = lines
            };
        }
    }
}
