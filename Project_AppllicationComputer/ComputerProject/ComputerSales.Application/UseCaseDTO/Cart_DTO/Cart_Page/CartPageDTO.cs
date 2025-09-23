using ComputerSales.Application.UseCaseDTO.Cart_DTO.Cart_Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCaseDTO.Cart_DTO.Cart_Page
{
    public class CartPageDTO
    {
        public int CartId { get; init; }
        public int ItemsCount { get; init; }
        public decimal Subtotal { get; init; }
        public decimal DiscountTotal { get; init; }
        public decimal ShippingFee { get; init; }
        public decimal TaxTotal { get; init; }
        public decimal GrandTotal { get; init; }
        public IReadOnlyList<CartItemsDTO> Lines { get; init; } = [];
    }
}
