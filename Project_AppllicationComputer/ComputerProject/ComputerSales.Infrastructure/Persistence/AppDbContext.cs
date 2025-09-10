using ComputerSales.Domain.Entity.ECategory;
using ComputerSales.Domain.Entity.EOptional;
using ComputerSales.Domain.Entity.EProduct;
using ComputerSales.Domain.Entity.EProvider;
using ComputerSales.Domain.Entity.EVariant;
﻿using ComputerSales.Domain.Entity;
using ComputerSales.Domain.Entity.E_Order;
using ComputerSales.Domain.Entity.EAccount;
using ComputerSales.Domain.Entity.ECart;
using ComputerSales.Domain.Entity.ECustomer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<ProductVariant> productVariants => Set<ProductVariant>();
        public DbSet<ProductProtection> productProtections => Set<ProductProtection>();
        public DbSet<ProductOverview> productOverviews => Set<ProductOverview>();
        public DbSet<ProductOptionType> productOptionTypes => Set<ProductOptionType>();
        public DbSet<OptionType> optionTypes => Set<OptionType>();
        public DbSet<OptionalValue> optionalValues => Set<OptionalValue>();
        public DbSet<VariantImage> variantImages => Set<VariantImage>();
        public DbSet<VariantOptionValue> variantOptionValues => Set<VariantOptionValue>();
        public DbSet<VariantPrice> variantPrices => Set<VariantPrice>();
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder b)
        {
            // Tự áp dụng tất cả Fluent API trong thư mục Persistence/Configuration
            b.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
