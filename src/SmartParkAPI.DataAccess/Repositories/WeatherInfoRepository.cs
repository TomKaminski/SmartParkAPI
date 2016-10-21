using System;
using Microsoft.EntityFrameworkCore;
using SmartParkAPI.DataAccess.Common;
using SmartParkAPI.DataAccess.Interfaces;
using SmartParkAPI.Model.Concrete;

namespace SmartParkAPI.DataAccess.Repositories
{
    public class WeatherInfoRepository  : GenericRepository<WeatherInfo, Guid>, IWeatherInfoRepository
    {
        private readonly DbSet<WeatherInfo> _dbSet;

        public WeatherInfoRepository(IDatabaseFactory factory)
            : base(factory)
        {
            _dbSet = factory.Get().Set<WeatherInfo>();
        }
    }
}
