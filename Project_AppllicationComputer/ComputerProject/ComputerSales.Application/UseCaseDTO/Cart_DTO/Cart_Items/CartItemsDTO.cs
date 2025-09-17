using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCaseDTO.Cart_DTO.Cart_Items
{
    public class CartItemsDTO
    {
        public int CartItemId { get; init; }
        public string ImageUrl { get; init; } = "";
        public string Name { get; init; } = "";
        public string SKU { get; init; } = "";
        public string OptionSummary { get; init; } = "";
        public int Quantity { get; init; }
        public int? PerItemLimit { get; init; }
        public decimal ListPrice { get; init; }
        public decimal SalePrice { get; init; }
        public decimal Savings => Math.Max(0, ListPrice - SalePrice);
        public int? SavingsPercent => ListPrice > 0 ? (int)Math.Round((Savings / ListPrice) * 100) : null;
        public bool IsChildService { get; init; }
        public int? ParentItemId { get; init; }

    }
}
