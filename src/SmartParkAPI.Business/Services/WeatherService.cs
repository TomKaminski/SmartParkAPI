using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json;
using SmartParkAPI.Business.HelperClasses;
using SmartParkAPI.Business.Services.Base;
using SmartParkAPI.Contracts.Common;
using SmartParkAPI.Contracts.DTO.Weather;
using SmartParkAPI.Contracts.Services;
using SmartParkAPI.DataAccess.Common;
using SmartParkAPI.DataAccess.Interfaces;
using Weather = SmartParkAPI.Model.Concrete;

namespace SmartParkAPI.Business.Services
{
    public class WeatherService : EntityService<WeatherDto, Weather.Weather, Guid>, IWeatherService
    {
        private readonly IWeatherRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public WeatherService(IWeatherRepository repository, IUnitOfWork unitOfWork, IMapper mapper) : base(repository, unitOfWork, mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResult<WeatherDto>> GetLatestWeatherDataAsync()
        {
            var latestWeather = await _repository.GetMostRecentWeather();
            if (latestWeather== null || latestWeather.ValidToDate < DateTime.Now)
            {
                return UpdateWeatherAndReturn(_mapper.Map<WeatherDto>(latestWeather));
            }
            return ServiceResult<WeatherDto>.Success(_mapper.Map<WeatherDto>(latestWeather));
        }

        public ServiceResult<WeatherDto> UpdateWeatherAndReturn(WeatherDto latestWeather)
        {
            var request = (HttpWebRequest)WebRequest.Create("http://api.openweathermap.org/data/2.5/weather?id=3103402&appid=0db985dfe762e26f24741f0393273666");

            try
            {
                var response = request.GetResponse();
                using (var responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        var reader = new StreamReader(responseStream, Encoding.UTF8);
                        var obj = JsonConvert.DeserializeObject<WeatherHelper>(reader.ReadToEnd());
                        var weatherDto = _mapper.Map<WeatherDto>(obj);
                        var weather = _mapper.Map<Weather.Weather>(weatherDto);
                        _repository.Add(weather);
                        _unitOfWork.Commit();
                        return ServiceResult<WeatherDto>.Success(weatherDto);
                    }
                    return ServiceResult<WeatherDto>.Success(new WeatherDto());
                }
            }
            catch
            {
                return ServiceResult<WeatherDto>.Success(latestWeather);
            }
            
        }
    }
}
