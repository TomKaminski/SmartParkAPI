using SmartParkAPI.Models.Base;

namespace SmartParkAPI.Models.Admin.PriceTreshold
{
    public class AdminPriceTresholdListItemViewModel : SmartParkListBaseViewModel
    {
        public int Id { get; set; }
        public int MinCharges { get; set; }
        public decimal PricePerCharge { get; set; }
        public int NumOfOrders { get; set; }
        public bool IsDeleted { get; set; }
    }
}
