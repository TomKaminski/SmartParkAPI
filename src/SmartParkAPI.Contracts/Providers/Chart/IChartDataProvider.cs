using System.Threading.Tasks;
using SmartParkAPI.Contracts.DTO.Chart;

namespace SmartParkAPI.Contracts.Providers.Chart
{
    public interface IChartDataProvider
    {
        Task<ChartListDto> GetChartData(ChartRequestDto request);
    }
}
