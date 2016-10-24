using System.Linq;
using AutoMapper;
using SmartParkAPI.Contracts.DTO.User;
using SmartParkAPI.Contracts.DTO.UserPreferences;
using SmartParkAPI.Model.Concrete;
using SmartParkAPI.Shared.Helpers;

namespace SmartParkAPI.Resolver.Mappings
{
    public class UserBackendMappings : Profile
    {
        public UserBackendMappings()
        {
            CreateMap<User, UserBaseDto>();
            CreateMap<UserBaseDto, User>();

            CreateMap<User, UserAdminDto>()
                .ForMember(x => x.OrdersCount, opt => opt.MapFrom(x => x.Orders.Count))
                .ForMember(x => x.GateUsagesCount, opt => opt.MapFrom(x => x.GateUsages.Count))
                .ForMember(x => x.ImgId, opt => opt.MapFrom(a => a.UserPreferences.ProfilePhotoId.ToString()))
                .ForMember(x => x.Orders, opt => opt.MapFrom(a => a.Orders.OrderByDescending(x => x.Date).Take(3)))
                ;

            CreateMap<UserPreferences, UserPreferencesDto>();
            CreateMap<UserPreferencesDto, UserPreferences>();
        }
    }
}
