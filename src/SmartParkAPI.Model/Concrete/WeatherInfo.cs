using System;
using SmartParkAPI.Model.Common;

namespace SmartParkAPI.Model.Concrete
{
    public class WeatherInfo : Entity<Guid>
    {
        public int WeatherConditionId { get; set; }
        public string WeatherMain { get; set; }
        public string WeatherDescription { get; set; }

        public Guid WeatherId { get; set; }
        public virtual Weather Weather { get; set; }
    }
}
