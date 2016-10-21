using AutoMapper;
using SmartParkAPI.Contracts.DTO.PriceTreshold;
using SmartParkAPI.Model.Concrete;
using SmartParkAPI.Shared.Helpers;

namespace SmartParkAPI.Resolver.Mappings
{
    public class PriceTresholdBackendMappings : Profile
    {
        protected override void Configure()
        {
            CreateMap<PriceTreshold, PriceTresholdBaseDto>().IgnoreNotExistingProperties();

            CreateMap<PriceTreshold, PriceTresholdAdminDto>()
                .ForMember(x => x.NumOfOrders, opt => opt.MapFrom(k => k.Orders.Count))
                .IgnoreNotExistingProperties();

            CreateMap<PriceTresholdBaseDto, PriceTreshold>().IgnoreNotExistingProperties();
        }
    }
}
