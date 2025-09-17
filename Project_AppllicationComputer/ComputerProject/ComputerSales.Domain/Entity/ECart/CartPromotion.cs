namespace ComputerSales.Domain.Entity.ECart
{
    public class CartPromotion
    {
        public int Id { get; set; }
        public int CartID { get; set; }
        public int PromotionId { get; set; }
        public decimal AppliedAmount { get; set; } // snapshot số tiền đã trừ

        public Cart Cart { get; set; } = null!;
        public Promotion Promotion { get; set; } = null!;
    }
}
