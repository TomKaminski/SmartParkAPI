using System;

namespace SmartParkAPI.Shared.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime LastDayOfMonth(this DateTime date)
        {
            return date.FirstDayOfMonth().AddMonths(1).AddDays(-1).Date;
        }

        public static DateTime FirstDayOfMonth(this DateTime date)
        {
            return (new DateTime(date.Year, date.Month, 1)).Date;
        }
    }
}
