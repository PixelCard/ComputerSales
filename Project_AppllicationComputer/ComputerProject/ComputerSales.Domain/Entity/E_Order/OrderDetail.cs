using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Domain.Entity.E_Order
{
    public class OrderDetail
    {

        //Primary key 
        public int OrderID { get; set; }               
        public int? ProductID { get; set; }            
        public int ProductVariantID { get; set; } // biến thể ( có thể xem là thể loại ) 
        public int Quantity { get; set; }               

        //giá gốc
        public decimal UnitPrice { get; set; }         
        public decimal Discount { get; set; }       

        //giá bán
        public decimal TotalPrice { get; private set; }


        // có thể xem như là mã ISO
        public string SKU { get; set; }
        public string Name { get; set; }
        public string OptionSummary { get; set; }
        public string ImageUrl { get; set; }

        // Navigation
        public Order Order { get; set; }
        public Product Product { get; set; }
       public ProductVariant ProductVariant { get; set; }
    }
}
