using ComputerSalesProject_MVC.Areas.Admin.Models;

namespace ComputerSalesProject_MVC.Areas.Admin.Fake_data
{
    public static class ProductFakeData
    {
        public static List<ProductViewModel> GetProducts()
        {
            return new List<ProductViewModel>
        {
            new ProductViewModel
            {
                Title = "MSI 27\" 240 Hz OLED QHD Gaming Monitor MAG 271QPX QD-OLED E2",
                ImageUrl = "/images/msi-monitor.jpg",
                Price = 549.99m,
                OldPrice = 699.99m,
                DiscountPercent = 21,
                ShortDescription = "27'' QHD 2560x1440, 240Hz, OLED panel",
                PromoText = "$50 off w/ promo code GH8P23",
                Rating = 5,
                ReviewCount = 113,
                Badge = "Newegg Select"
            },
            new ProductViewModel
            {
                Title = "ASUS PRIME GeForce RTX 5070 Ti 16GB 256-Bit GDDR7 PCI Express",
                ImageUrl = "/images/asus-rtx5070ti.jpg",
                Price = 749.99m,
                OldPrice = 762.98m,
                DiscountPercent = 2,
                ShortDescription = "New NVIDIA Ada GPU architecture",
                PromoText = "",
                Rating = 4,
                ReviewCount = 120,
                Badge = ""
            },
            new ProductViewModel
            {
                Title = "SAMSUNG 990 PRO SSD 1TB PCIe 4.0 M.2 2280",
                ImageUrl = "/images/samsung-990pro.jpg",
                Price = 89.99m,
                OldPrice = 127.99m,
                DiscountPercent = 29,
                ShortDescription = "Gen4 NVMe SSD, up to 7450MB/s",
                PromoText = "",
                Rating = 5,
                ReviewCount = 774,
                Badge = ""
            },
            new ProductViewModel
            {
                Title = "HZG Gaming Desktop Computer PC, AMD Ryzen 7 5700G",
                ImageUrl = "/images/hzg-desktop.jpg",
                Price = 429.00m,
                OldPrice = 859.00m,
                DiscountPercent = 50,
                ShortDescription = "Ryzen 7, Radeon graphics, 16GB RAM",
                PromoText = "Free Gift + $10 promotional gift card",
                Rating = 4,
                ReviewCount = 63,
                Badge = ""
            },
            new ProductViewModel
            {
                Title = "Hasee T8 Pro-16\" GeForce RTX 5070 Laptop, Intel i7-14700HX",
                ImageUrl = "/images/hasee-laptop.jpg",
                Price = 1289.99m,
                OldPrice = 2499.99m,
                DiscountPercent = 48,
                ShortDescription = "16\" QHD 180Hz, 16GB + 1TB SSD",
                PromoText = "",
                Rating = 5,
                ReviewCount = 88,
                Badge = ""
            }
        };
        }
    }

}
