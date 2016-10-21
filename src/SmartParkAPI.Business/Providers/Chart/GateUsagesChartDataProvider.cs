using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartParkAPI.Contracts.DTO.Chart;
using SmartParkAPI.Contracts.Providers.Chart;
using SmartParkAPI.DataAccess.Interfaces;

namespace SmartParkAPI.Business.Providers.Chart
{
    public class GateUsagesChartDataProvider : BaseChartDataProvider, IGateUsagesChartDataProvider
    {
        private readonly IGateUsageRepository _gateUsagesRepository;

        public GateUsagesChartDataProvider(IGateUsageRepository gateUsagesRepository)
        {
            _gateUsagesRepository = gateUsagesRepository;
        }


        protected override async Task<Dictionary<DateTime, int>> GetData(ChartRequestDto request)
        {
            var gateUsages = await _gateUsagesRepository.GetAllAsync(x => x.UserId == request.UserId && x.DateOfUse >= request.DateRange.StartDate && x.DateOfUse <= request.DateRange.EndDate);
            return gateUsages.GroupBy(x => x.DateOfUse.Date).ToDictionary(x => x.Key, x => x.Count());
        }
    }
}
