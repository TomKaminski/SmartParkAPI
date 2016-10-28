using AutoMapper;
using SmartParkAPI.Contracts.DTO.UserDevice;
using SmartParkAPI.Model.Concrete;

namespace SmartParkAPI.Resolver.Mappings
{
    public class UserDeviceBackendMappings : Profile
    {
        public UserDeviceBackendMappings()
        {
            CreateMap<UserDevice, UserDeviceDto>();
            CreateMap<UserDeviceDto, UserDevice>();
        }
    }
}
