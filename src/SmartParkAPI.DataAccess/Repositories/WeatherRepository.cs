using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartParkAPI.DataAccess.Common;
using SmartParkAPI.DataAccess.Interfaces;
using SmartParkAPI.Model.Concrete;

namespace SmartParkAPI.DataAccess.Repositories
{
    public class WeatherRepository : GenericRepository<Weather, Guid>, IWeatherRepository
    {
        private readonly DbSet<Weather> _dbSet;

        public WeatherRepository(IDatabaseFactory factory)
            : base(factory)
        {
            _dbSet = factory.Get().Set<Weather>();
        }

        public async Task<Weather> GetMostRecentWeather()
        {
            return (await _dbSet.OrderByDescending(x => x.DateOfRead).Include(x => x.WeatherInfo).ToListAsync()).FirstOrDefault();
        }
    }
}
