using System;
using ParkingATHWeb.Shared.Enums;
using SmartParkAPI.Shared.Enums;
using SmartParkAPI.Shared.Helpers;

namespace SmartParkAPI.Contracts.DTO.Chart
{
    public class ChartRequestDto
    {
        public ChartRequestDto()
        { 
        }

        public ChartRequestDto(DateTime startDate, DateTime endDate, ChartType type, ChartGranuality granuality, int userId)
        {
            DateRange = new DateRange(startDate,endDate);
            Type = type;
            Granuality = granuality;
            UserId = userId;
        }
        public ChartGranuality Granuality { get; set; }
        public ChartType Type { get; set; }
        public DateRange DateRange { get; set; }
        public int UserId { get; set; }
    }
}
