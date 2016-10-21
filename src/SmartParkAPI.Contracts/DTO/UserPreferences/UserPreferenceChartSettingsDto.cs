using System;
using ParkingATHWeb.Shared.Enums;
using SmartParkAPI.Shared.Enums;

namespace SmartParkAPI.Contracts.DTO.UserPreferences
{
    public class UserPreferenceChartSettingsDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ChartGranuality Granuality { get; set; }
        public ChartType Type { get; set; }
        public int UserId { get; set; }
    }
}
