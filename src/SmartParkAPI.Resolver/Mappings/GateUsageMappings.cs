using AutoMapper;
using SmartParkAPI.Contracts.DTO.GateUsage;
using SmartParkAPI.Model.Concrete;
using SmartParkAPI.Shared.Helpers;

namespace SmartParkAPI.Resolver.Mappings
{
    public class GateUsageBackendMappings : Profile
    {
        protected override void Configure()
        {
            CreateMap<GateUsage, GateUsageBaseDto>().IgnoreNotExistingProperties();

            CreateMap<GateUsage, GateUsageAdminDto>()
                .ForMember(x => x.Initials, opt => opt.MapFrom(k => $"{k.User.Name} {k.User.LastName}"))
                .IgnoreNotExistingProperties();

            CreateMap<GateUsageBaseDto, GateUsage>().IgnoreNotExistingProperties();
        }
    }
}
