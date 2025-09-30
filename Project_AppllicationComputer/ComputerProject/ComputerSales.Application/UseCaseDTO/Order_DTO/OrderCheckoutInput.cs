using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCaseDTO.Order_DTO
{
    public enum PaymentKind { COD = 0, VNPAY = 1 }
    public sealed class OrderCheckoutInput
    {
        [Required, StringLength(150)]
        public string FullName { get; set; } = "";

        [Required, Phone]
        public string Phone { get; set; } = "";

        [EmailAddress]
        public string? Email { get; set; }

        [Required, StringLength(300)]
        public string Address { get; set; } = "";

        [StringLength(1000)]
        public string? Notes { get; set; }

        [Required]
        public PaymentKind Payment { get; set; } = PaymentKind.COD;
    }
}
