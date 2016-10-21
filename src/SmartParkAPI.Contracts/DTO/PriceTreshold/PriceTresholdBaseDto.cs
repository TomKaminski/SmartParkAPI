using SmartParkAPI.Contracts.Common;

namespace SmartParkAPI.Contracts.DTO.PriceTreshold
{
    public class PriceTresholdBaseDto:BaseDto<int>
    {
        public int MinCharges { get; set; }
        public decimal PricePerCharge { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class PrcAdminCreateInfo
    {
        public bool Recovered { get; set; }
        public bool ReplacedDefault { get; set; }
    }
}
