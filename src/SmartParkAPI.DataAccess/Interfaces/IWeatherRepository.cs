using System;
using System.Threading.Tasks;
using SmartParkAPI.DataAccess.Common;
using SmartParkAPI.Model.Concrete;

namespace SmartParkAPI.DataAccess.Interfaces
{
    public interface IWeatherRepository : IGenericRepository<Weather, Guid>, IDependencyRepository
    {
        Task<Weather> GetMostRecentWeather();
    }
}
