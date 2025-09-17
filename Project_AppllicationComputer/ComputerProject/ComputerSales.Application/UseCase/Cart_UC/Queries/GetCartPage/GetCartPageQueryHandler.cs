using ComputerSales.Application.Interface.Cart_Interface;
using ComputerSales.Application.UseCaseDTO.Cart_DTO.Cart_Items;
using ComputerSales.Application.UseCaseDTO.Cart_DTO.Cart_Page;
using ComputerSales.Domain.Entity.EProduct;
using ComputerSales.Domain.Entity.EVariant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCase.Cart_UC.Queries.GetCartPage
{
    public sealed class GetCartPageQueryHandler
    {
        private readonly ICartReadRespository _repo;
        public GetCartPageQueryHandler(ICartReadRespository repo) => _repo = repo;

        public async Task<CartPageDTO> Handle(GetCartPageQuery q, CancellationToken ct = default)
        {
            //1.Lấy giỏ hàng từ repo
            var cart = await _repo.GetByUserAsync(q.UserId, ct);
            if (cart == null) return new CartPageDTO();

            //2.Chuẩn bị dữ liệu variant
            var variantIds = cart.Items.Where(i => i.ProductVariantID.HasValue)
                                       .Select(i => i.ProductVariantID!.Value)
                                       .Distinct().ToArray(); //Lấy tất cả ProductVariantID trong giỏ sau đó bỏ vào 1 cái mảng

            var variants = await _repo.GetVariantsAsync(variantIds, ct); //dictionary<variantId → ProductVariant>

            //3.Hàm phụ chọn giá active

            //Trong một list VariantPrice, chọn cái đang hiệu lực (theo ValidFrom/ValidTo).

            //Nếu nhiều giá chồng nhau, lấy cái mới nhất.

            VariantPrice? Active(IEnumerable<VariantPrice> ps) =>
                ps.OrderByDescending(p => p.ValidFrom ?? DateTime.MinValue)
                  .FirstOrDefault(p => (!p.ValidFrom.HasValue || p.ValidFrom <= DateTime.UtcNow)
                                    && (!p.ValidTo.HasValue || p.ValidTo >= DateTime.UtcNow));


            //4.Hàm phụ dựng chuỗi OptionSummary

            /*
                Đi qua từng VariantOptionValue (ví dụ: Style=Faster, Options=4TB).

                Nhóm theo OptionType.Name, ghép value theo SortOrder.

                Kết quả: "Style: Faster • Options: 4TB".

             */

            string BuildOptionSummary(ProductVariant? v)
            {
                if (v == null) return "";
                var parts = v.VariantOptionValues
                    .Select(x => new { T = x.OptionalValue!.OptionType!.Name, V = x.OptionalValue.Value, O = x.OptionalValue.SortOrder })
                    .OrderBy(x => x.O).GroupBy(x => x.T)
                    .Select(g => $"{g.Key}: {string.Join(", ", g.Select(x => x.V))}");
                return string.Join(" • ", parts);
            }


            //5.Map từng CartItem → CartLineVM
            var lines = cart.Items.Select(i =>
            {
                variants.TryGetValue(i.ProductVariantID ?? -1, out var v);
                var ap = Active(v?.VariantPrices ?? Array.Empty<VariantPrice>());
                var list = ap?.Price ?? i.UnitPrice;
                var sale = ap?.DiscountPrice ?? i.UnitPrice;

                return new CartItemsDTO
                {
                    CartItemId = i.ID,
                    ImageUrl = v?.VariantImages.FirstOrDefault()?.Url ?? i.ImageUrl,
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
            }).ToList();

            // Tổng theo SalePrice của item chính
            var subtotal = lines.Where(l => !l.IsChildService).Sum(l => l.SalePrice * l.Quantity);

            // Không ghi DB trong Query; chỉ mirror từ entity
            return new CartPageDTO
            {
                CartId = cart.ID,
                ItemsCount = cart.Items.Count,
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
