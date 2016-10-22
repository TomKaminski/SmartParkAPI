using System;
using ParkingATHWeb.Shared.Enums;

namespace SmartParkAPI.Models.Portal.Chart
{
    public class ChartPreferencesReturnModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ChartType Granuality { get; set; }
        public string LabelStartDate { get; set; }
        public string LabelEndDate { get; set; }
    }
}
