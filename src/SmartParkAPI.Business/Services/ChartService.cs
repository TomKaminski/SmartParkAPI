using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ParkingATHWeb.Shared.Enums;
using SmartParkAPI.Business.Providers.Chart;
using SmartParkAPI.Contracts.Common;
using SmartParkAPI.Contracts.DTO.Chart;
using SmartParkAPI.Contracts.Providers.Chart;
using SmartParkAPI.Contracts.Services;
using SmartParkAPI.DataAccess;
using SmartParkAPI.DataAccess.Repositories;
using SmartParkAPI.Shared.Enums;

namespace SmartParkAPI.Business.Services
{
    public class ChartService : IChartService
    {
        private readonly IGateUsagesChartDataProvider _gateUsagesChartDataProvider;
        private readonly IOrdersChartDataProvider _ordersChartDataProvider;

        public ChartService(IGateUsagesChartDataProvider gateUsagesChartDataProvider, IOrdersChartDataProvider ordersChartDataProvider)
        {
            _gateUsagesChartDataProvider = gateUsagesChartDataProvider;
            _ordersChartDataProvider = ordersChartDataProvider;
        }

        public ChartService()
        {
            _gateUsagesChartDataProvider = new GateUsagesChartDataProvider(new GateUsageRepository(new DatabaseFactory()));
            _ordersChartDataProvider = new OrdersDataChartProvider(new OrderRepository(new DatabaseFactory()));
        }

        public async Task<ServiceResult<ChartListDto>> GetDataAsync(ChartRequestDto request)
        {
            switch (request.Type)
            {
                case ChartType.GateOpenings:
                    return ServiceResult<ChartListDto>.Success(await _gateUsagesChartDataProvider.GetChartData(request));
                case ChartType.Orders:
                    return ServiceResult<ChartListDto>.Success(await _ordersChartDataProvider.GetChartData(request));
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public async Task<ServiceResult<Dictionary<ChartType,ChartListDto>, ChartRequestDto>> GetDefaultDataAsync(int userId)
        {
            var chartRequest = new ChartRequestDto(DateTime.Today.AddDays(-6), DateTime.Today.AddDays(1).AddSeconds(-1),
                ChartType.GateOpenings,ChartGranuality.PerDay, userId);
            var gateOpeningsData = await GetDataAsync(chartRequest);
            chartRequest.Type = ChartType.Orders;
            var orderData = await GetDataAsync(chartRequest);
            var resultDictionary = new Dictionary<ChartType,ChartListDto>()
            {
                {ChartType.GateOpenings, gateOpeningsData.IsValid ? gateOpeningsData.Result : new ChartListDto() },
                {ChartType.Orders, orderData.IsValid ? orderData.Result : new ChartListDto() }
            };
            return ServiceResult<Dictionary<ChartType, ChartListDto>, ChartRequestDto>.Success(resultDictionary, chartRequest);
        }
    }
}
