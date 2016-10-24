using AutoMapper;
using SmartParkAPI.Contracts.DTO.Order;
using SmartParkAPI.Model.Concrete;
using SmartParkAPI.Shared.Enums;
using SmartParkAPI.Shared.Helpers;

namespace SmartParkAPI.Resolver.Mappings
{
    public class OrderBackendMappings : Profile
    {
        public OrderBackendMappings()
        {
            CreateMap<OrderBaseDto, Order>()
                 .ForMember(x => x.OrderState, opt => opt.UseValue(OrderStatus.Pending))
                 ;

            CreateMap<Order, OrderAdminDto>()
                .ForMember(x => x.LastName, opt => opt.MapFrom(k => k.User.LastName))
                .ForMember(x => x.Name, opt => opt.MapFrom(k => k.User.Name))
                .ForMember(x => x.PricePerCharge, a => a.MapFrom(m => m.Price / m.NumOfCharges))
                ;

            CreateMap<Order, OrderBaseDto>()
                .ForMember(x => x.PricePerCharge, a => a.MapFrom(m => m.Price / m.NumOfCharges))
                ;
        }
    }
}
