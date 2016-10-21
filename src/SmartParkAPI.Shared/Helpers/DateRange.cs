using System;
using System.Collections.Generic;
using System.Linq;
using SmartParkAPI.Shared.Extensions;

namespace SmartParkAPI.Shared.Helpers
{
    public class DateRange
    {
        public DateRange(DateTime startDate, DateTime endDate)
        {
            Dates = new List<DateTime>();
            var date = startDate;
            do
            {
                Dates.Add(date);
                date = date.AddDays(1);
            } while (date < endDate);
            StartDate = startDate;
            EndDate = endDate;
        }
        public List<DateTime> Dates { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<DateRange> GetWeekGroups()
        {
            var startDateDayOfWeek = StartDate.DayOfWeek;
            var firstRangeLength = startDateDayOfWeek == DayOfWeek.Sunday ? 0 : 7 - (int)startDateDayOfWeek;
            var weekGroup = new List<DateRange>
            {
                new DateRange(StartDate, StartDate.AddDays(firstRangeLength))
            };
            var dynamicStartDate = StartDate.AddDays(firstRangeLength + 1);
            while (dynamicStartDate <= EndDate)
            {
                var dynamicWeekEndDate = dynamicStartDate.AddDays(6);
                weekGroup.Add(dynamicWeekEndDate > EndDate
                    ? new DateRange(dynamicStartDate, EndDate)
                    : new DateRange(dynamicStartDate, dynamicWeekEndDate));

                dynamicStartDate = dynamicWeekEndDate.AddDays(1);
            }
            return weekGroup;
        }

        public List<DateRange> GetMonthGroups()
        {
            var result = new List<DateRange>();            

            var tempStartDate = StartDate;
            while(tempStartDate.Month <= EndDate.Month)
            {
                result.Add(new DateRange(tempStartDate, tempStartDate.LastDayOfMonth()));
                tempStartDate = tempStartDate.LastDayOfMonth().AddDays(1);
            }
            return result;
        }

        public List<DateRange> GetDailyGroups()
        {
            return Dates.Select(t => new DateRange(t, t)).ToList();
        }
    }
}
