using System;
using System.Threading.Tasks;
using SmartParkAPI.Contracts.Common;
using SmartParkAPI.Contracts.DTO.Weather;
using SmartParkAPI.Contracts.Services.Base;

namespace SmartParkAPI.Contracts.Services
{
    public interface IWeatherService : IEntityService<WeatherDto,Guid>, IDependencyService
    {
        Task<ServiceResult<WeatherDto>> GetLatestWeatherDataAsync();
    }
}
