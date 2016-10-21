using System;
using SmartParkAPI.Contracts.Common;

namespace SmartParkAPI.Contracts.DTO.WeatherInfo
{
    public class WeatherInfoDto : BaseDto<Guid>
    {
        public int WeatherConditionId { get; set; }
        public string WeatherMain { get; set; }
        public string WeatherDescription { get; set; }
        public Guid WeatherId { get; set; }
    }
}
