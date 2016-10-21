namespace SmartParkAPI.Contracts.DTO.Payments
{
    public class OrderPaymentInfo
    {
        public decimal TotalAmount { get; set; }
        public decimal PricePerCharge { get; set; }
        public int PriceTresholdId { get; set; }
    }
}