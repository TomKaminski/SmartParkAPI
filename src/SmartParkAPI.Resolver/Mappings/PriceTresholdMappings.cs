using AutoMapper;
using SmartParkAPI.Contracts.DTO.PriceTreshold;
using SmartParkAPI.Model.Concrete;
using SmartParkAPI.Shared.Helpers;

namespace SmartParkAPI.Resolver.Mappings
{
    public class PriceTresholdBackendMappings : Profile
    {
        public PriceTresholdBackendMappings()
        {
            CreateMap<PriceTreshold, PriceTresholdBaseDto>();

            CreateMap<PriceTreshold, PriceTresholdAdminDto>()
                .ForMember(x => x.NumOfOrders, opt => opt.MapFrom(k => k.Orders.Count))
                ;

            CreateMap<PriceTresholdBaseDto, PriceTreshold>();
        }
    }
}
