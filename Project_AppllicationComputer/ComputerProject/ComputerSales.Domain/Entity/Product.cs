using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Domain.Entity
{
    public class Product
    {
        private Product() { } // EF cần

        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; private set; } = default!;
        public string Description { get; private set; } = default!;

        public static Product Create(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required");
            return new Product { Name = name.Trim(), Description = (description ?? "").Trim() };
        }
    }
}
