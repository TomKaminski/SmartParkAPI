using System.Collections.Generic;

namespace SmartParkAPI.Models.Portal.Weather
{
    public class WeatherDataViewModel
    {
        public int Clouds { get; set; }
        public double Temperature { get; set; }
        public double Pressure { get; set; }
        public double Humidity { get; set; }
        public string DateOfRead { get; set; }
        public int HourOfRead { get; set; }
        public List<WeatherInfoDataViewModel> WeatherInfo { get; set; }
    }

    public class WeatherInfoDataViewModel
    {
        public string WeatherMain { get; set; }
        public string WeatherDescription { get; set; }
        public int WeatherId { get; set; }
    }
}
