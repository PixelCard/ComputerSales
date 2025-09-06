using ComputerSales.Domain.Entity.ECategory;
using ComputerSales.Domain.Entity.EOptional;
using ComputerSales.Domain.Entity.EProduct;
using ComputerSales.Domain.Entity.EProvider;
using ComputerSales.Domain.Entity.EVariant;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Accessories> Accessories => Set<Accessories>();
        public DbSet<Provider> Providers => Set<Provider>();
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
