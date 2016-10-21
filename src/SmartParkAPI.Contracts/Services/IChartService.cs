using System.Collections.Generic;
using System.Threading.Tasks;
using ParkingATHWeb.Shared.Enums;
using SmartParkAPI.Contracts.Common;
using SmartParkAPI.Contracts.DTO.Chart;
using SmartParkAPI.Contracts.Services.Base;

namespace SmartParkAPI.Contracts.Services
{
    public interface IChartService : IDependencyService
    {
        Task<ServiceResult<ChartListDto>> GetDataAsync(ChartRequestDto request);
        Task<ServiceResult<Dictionary<ChartType, ChartListDto>, ChartRequestDto>> GetDefaultDataAsync(int userId);
    }
}
