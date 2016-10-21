using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartParkAPI.Contracts.DTO.Chart;
using SmartParkAPI.Shared.Enums;
using SmartParkAPI.Shared.Helpers;

namespace SmartParkAPI.Business.Providers.Chart
{
    public abstract class BaseChartDataProvider : IBaseChartDataProvider
    {
        protected List<DateRange> TransformDateRange(ChartGranuality granuality, DateRange requestDateRange)
        {
            switch (granuality)
            {
                case ChartGranuality.PerDay:
                    return requestDateRange.GetDailyGroups();
                case ChartGranuality.PerWeek:
                    return requestDateRange.GetWeekGroups();
                case ChartGranuality.PerMonth:
                    return requestDateRange.GetMonthGroups();
                default:
                    throw new ArgumentOutOfRangeException(nameof(granuality), granuality, null);
            }
        }

        protected IEnumerable<ChartElement> GroupAndSumGateUsagesByDateRange(IEnumerable<DateRange> dateRanges, Dictionary<DateTime, int> dailyAggregatedGateUsages, ChartGranuality granuality)
        {
            return dateRanges.Select(dateRange => new ChartElement(dateRange.StartDate, dailyAggregatedGateUsages.Where(x => x.Key >= dateRange.StartDate && x.Key <= dateRange.EndDate).Sum(x => x.Value), granuality)).ToList();
        }

        protected abstract Task<Dictionary<DateTime,int>> GetData(ChartRequestDto request);

        public async Task<ChartListDto> GetChartData(ChartRequestDto request)
        {
            var transformedGroup = TransformDateRange(request.Granuality, request.DateRange);
            var resultChartElements = GroupAndSumGateUsagesByDateRange(transformedGroup, await GetData(request), request.Granuality);

            return new ChartListDto
            {
                UserId = request.UserId,
                Granuality = request.Granuality,
                Type = request.Type,
                Elements = resultChartElements
            };
        }

    }

    public interface IBaseChartDataProvider
    {
    }
}
