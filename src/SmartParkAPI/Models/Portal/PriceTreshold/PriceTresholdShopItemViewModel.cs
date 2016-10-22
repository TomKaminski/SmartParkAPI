namespace SmartParkAPI.Models.Portal.PriceTreshold
{
    public class PriceTresholdShopItemViewModel
    {
        public int MinCharges { get; set; }
        public decimal PricePerCharge { get; set; }
        public string PriceLabel { get; set; }
        public int PercentDiscount { get; set; }
        public bool IsDeafult { get; set; }
    }
}
