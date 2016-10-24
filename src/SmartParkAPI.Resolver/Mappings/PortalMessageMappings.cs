using System;
using AutoMapper;
using SmartParkAPI.Contracts.DTO.PortalMessage;
using SmartParkAPI.Model.Concrete;
using SmartParkAPI.Shared.Enums;
using SmartParkAPI.Shared.Helpers;

namespace SmartParkAPI.Resolver.Mappings
{
    public class PortalMessageBackendMappings : Profile
    {
        public PortalMessageBackendMappings()
        {
            CreateMap<PortalMessageDto, PortalMessage>()
                .ForMember(x => x.PortalMessageType, a => a.MapFrom(s => Convert.ToInt32(s.PortalMessageType)))
                .ForMember(x => x.Title, a => a.MapFrom(x => !x.Starter ? null : $"Wiadomość do działu pomocy - {DateTime.Today.ToString("dd.MM.yyyy")} - {DateTime.Now.TimeOfDay.Hours}{DateTime.Now.TimeOfDay.Minutes}{DateTime.Now.TimeOfDay.Seconds}{DateTime.Now.TimeOfDay.Milliseconds}"))
                ;

            CreateMap<PortalMessage, PortalMessageDto>()
                .ForMember(x => x.PortalMessageType, a => a.MapFrom(s => (PortalMessageEnum)s.PortalMessageType))
                ;

            CreateMap<User, PortalMessageUserDto>()
                .ForMember(x => x.ImgId, opt => opt.MapFrom(a => (a.UserPreferences.ProfilePhotoId == null || a.UserPreferences.ProfilePhotoId == Guid.Empty) ? "avatar-placeholder" : a.UserPreferences.ProfilePhotoId.ToString()))
                .ForMember(x => x.Initials, opt => opt.MapFrom(a => $"{a.Name} {a.LastName}"))
                ;
        }

    }
}
