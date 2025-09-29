namespace ComputerSales.Application.Payment.VNPAY.Entity
{
    public class PaymentInformation
    {
        public int OrderId { get; set; }           // không dùng ở flow này
        public string TxnRef { get; set; } = "";   // dùng làm vnp_TxnRef
        public decimal Amount { get; set; }
        public string? Name { get; set; }
        public string? OrderDescription { get; set; }
        public string? OrderType { get; set; }
    }
}
