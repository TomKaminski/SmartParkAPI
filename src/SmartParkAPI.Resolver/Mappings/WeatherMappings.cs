using System;
using AutoMapper;
using SmartParkAPI.Business.HelperClasses;
using SmartParkAPI.Contracts.DTO.Weather;
using SmartParkAPI.Contracts.DTO.WeatherInfo;
using SmartParkAPI.Model.Concrete;
using SmartParkAPI.Shared.Helpers;
using Weather = SmartParkAPI.Model.Concrete.Weather;

namespace SmartParkAPI.Resolver.Mappings
{
    public class WeatherBackendMappings : Profile
    {
        protected override void Configure()
        {
            CreateMap<WeatherDto, Weather>().IgnoreNotExistingProperties();

            CreateMap<Weather, WeatherDto>()
                .IgnoreNotExistingProperties();

            CreateMap<Business.HelperClasses.Weather, WeatherInfoDto>()
                .ForMember(x => x.WeatherConditionId, src => src.MapFrom(a => a.id))
                .ForMember(x => x.WeatherDescription, src => src.MapFrom(a => a.description))
                .ForMember(x => x.WeatherMain, src => src.MapFrom(a => a.main))
                .ForMember(x => x.Id, opt => opt.Ignore())
                .IgnoreNotExistingProperties();


            CreateMap<WeatherInfo, WeatherInfoDto>()
               .IgnoreNotExistingProperties();

            CreateMap<WeatherInfoDto, WeatherInfo>()
                .IgnoreNotExistingProperties();

            CreateMap<Business.HelperClasses.Weather, WeatherInfo>()
                .ForMember(x => x.WeatherDescription, src => src.MapFrom(a => a.description))
                .ForMember(x => x.WeatherConditionId, src => src.MapFrom(a => a.id))
                .ForMember(x => x.WeatherMain, src => src.MapFrom(a => a.main))
                .ForMember(x => x.Id, opt => opt.Ignore())
                .IgnoreNotExistingProperties();

            CreateMap<WeatherHelper, WeatherDto>()
                .ForMember(x => x.DateOfRead, opt => opt.UseValue(DateTime.Now))
                .ForMember(x => x.ValidToDate, opt => opt.UseValue(DateTime.Now.AddMinutes(30)))
                .ForMember(x => x.Humidity, opt => opt.MapFrom(m => m.main.humidity))
                .ForMember(x => x.Pressure, opt => opt.MapFrom(m => m.main.pressure))
                .ForMember(x => x.Temperature, opt => opt.MapFrom(m => Math.Round(m.main.temp - 273.15, 0)))
                .ForMember(x => x.WeatherInfo, opt => opt.MapFrom(m => m.weather))
                .ForMember(x => x.Clouds, opt => opt.MapFrom(src => src.clouds.all))
                .ForMember(x => x.Id, opt => opt.Ignore());
        }
    }
}
