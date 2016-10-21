using System;
using System.Collections.Generic;
using SmartParkAPI.Model.Common;

namespace SmartParkAPI.Model.Concrete
{
    public class Weather : Entity<Guid>
    {
        public Weather()
        {
            WeatherInfo = new List<WeatherInfo>();
        }

        public int Clouds { get; set; }
        public double Temperature { get; set; }
        public double Pressure { get; set; }
        public double Humidity { get; set; }
        public DateTime DateOfRead { get; set; }
        public DateTime ValidToDate { get; set; }

        public virtual List<WeatherInfo> WeatherInfo { get; set; }
    }
}
