using System;
using SmartParkAPI.Shared.Enums;

namespace SmartParkAPI.Contracts.DTO.Chart
{
    public class ChartElement
    {
        public ChartElement(DateTime startDate, int nodeValue, ChartGranuality granuality)
        {
            StartDate = startDate;
            NodeValue = nodeValue;
            switch (granuality)
            {
                case ChartGranuality.PerDay:
                    DateLabel = startDate.ToString("dd.MM");
                    break;
                case ChartGranuality.PerWeek:
                    {
                        var endDate = startDate.AddDays(6);
                        DateLabel = $"{startDate.ToString("dd.MM")}-{endDate.ToString("dd.MM")}";
                        break;

                    }
                case ChartGranuality.PerMonth:
                    {
                        DateLabel = $"{startDate.ToString("MM.yyyy")}";
                        break;
                        ;
                    }
            }
        }

        public DateTime StartDate { get; set; }
        public string DateLabel { get; set; }
        public int NodeValue { get; set; }
    }
}
