using ParkingATHWeb.Shared.Enums;
using SmartParkAPI.Shared.Enums;

namespace SmartParkAPI.Models.Portal.Chart
{
    public class ChartDataReturnModel
    {
        public string[] Labels { get; set; }
        public int[] Data { get; set; }
        public ChartType Type { get; set; }
        public ChartGranuality Granuality { get; set; }
        public int UserId { get; set; }
    }
}
