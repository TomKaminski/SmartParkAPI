using AutoMapper;
using SmartParkAPI.Contracts.DTO.GateUsage;
using SmartParkAPI.Model.Concrete;
using SmartParkAPI.Shared.Helpers;

namespace SmartParkAPI.Resolver.Mappings
{
    public class GateUsageBackendMappings : Profile
    {
        public GateUsageBackendMappings()
        {
            CreateMap<GateUsage, GateUsageBaseDto>();

            CreateMap<GateUsage, GateUsageAdminDto>()
                .ForMember(x => x.Initials, opt => opt.MapFrom(k => $"{k.User.Name} {k.User.LastName}"))
                ;

            CreateMap<GateUsageBaseDto, GateUsage>();
        }
    }
}
