using SmartParkAPI.Model.Concrete;

namespace SmartParkAPI.DataAccess.Tests.Base
{
    public abstract class DataAccessTestBase
    {
        protected static PriceTreshold GetPriceTreshold()
        {
            return new PriceTreshold
            {
                MinCharges = 5,
                PricePerCharge = 5.5m
            };
        }
    }
}
