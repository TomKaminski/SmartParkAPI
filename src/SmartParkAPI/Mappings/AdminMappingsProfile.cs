using AutoMapper;
using SmartParkAPI.Contracts.DTO.GateUsage;
using SmartParkAPI.Contracts.DTO.Order;
using SmartParkAPI.Contracts.DTO.PriceTreshold;
using SmartParkAPI.Contracts.DTO.User;
using SmartParkAPI.Models.Admin.GateUsage;
using SmartParkAPI.Models.Admin.Order;
using SmartParkAPI.Models.Admin.PriceTreshold;
using SmartParkAPI.Models.Admin.User;
using SmartParkAPI.Shared.Enums;
using SmartParkAPI.Shared.Helpers;

namespace SmartParkAPI.Mappings
{
    public class AdminMappingsProfile : Profile
    {
        public AdminMappingsProfile()
        {

            CreateMap<GateUsageAdminDto, AdminGateUsageListItemViewModel>()
                  .ForMember(x => x.Initials, s => s.MapFrom(a => a.Initials))
                  .ForMember(x => x.Date, m => m.MapFrom(s => s.DateOfUse.ToString("dd MMMM yyyy")))
                  .ForMember(x => x.Time, m => m.MapFrom(s => s.DateOfUse.ToString("HH:mm")))
                  ;

            CreateMap<PriceTresholdAdminDto, AdminPriceTresholdListItemViewModel>();
            CreateMap<UserAdminDto, AdminUserListItemViewModel>()
                .ForMember(x => x.Initials, a => a.MapFrom(s => $"{s.Name} {s.LastName}"))
                .ForMember(x => x.CreateDateLabel, a => a.MapFrom(s => s.CreateDate.ToString("dd-MM-yyyy hh:mm")))
                .ForMember(x => x.LastUserOrders, a => a.MapFrom(s => s.Orders))
                ;


            CreateMap<AdminUserEditViewModel, UserBaseDto>();
            CreateMap<AdminPriceTresholdCreateViewModel, PriceTresholdBaseDto>();
            CreateMap<AdminPriceTresholdEditViewModel, PriceTresholdBaseDto>();

            CreateMap<PriceTresholdBaseDto, AdminPriceTresholdListItemViewModel>();

            CreateMap<OrderAdminDto, AdminOrderListItemViewModel>()
               .ForMember(x => x.Price, a => a.MapFrom(s => s.Price.ToString("#.00")))
               .ForMember(x => x.PricePerCharge, a => a.MapFrom(s => s.PricePerCharge.ToString("#.00")))
               .ForMember(x => x.Date, a => a.MapFrom(s => s.Date.ToString("dd.MM.yyyy")))
               .ForMember(x => x.Time, a => a.MapFrom(s => s.Date.ToString("HH:mm")))
               .ForMember(x => x.Initials, a => a.MapFrom(s => $"{s.Name} {s.LastName}"))
               .AfterMap((src, dest) =>
               {
                   switch (src.OrderState)
                   {
                       case OrderStatus.Completed:
                           dest.OrderState = "Sfinalizowane";
                           dest.OrderStateStyle = "order-success";
                           break;
                       case OrderStatus.Canceled:
                           dest.OrderState = "Anulowane";
                           dest.OrderStateStyle = "order-canceled";
                           break;
                       case OrderStatus.Rejected:
                           dest.OrderState = "Odrzucone";
                           dest.OrderStateStyle = "order-rejected";
                           break;
                       case OrderStatus.Pending:
                           dest.OrderState = "Oczekujące";
                           dest.OrderStateStyle = "order-pending";
                           break;
                   }
                   switch (src.OrderPlace)
                   {
                       case OrderPlace.Panel:
                           dest.OrderPlace = "Panel zakupowy";
                           break;
                       case OrderPlace.Website:
                           dest.OrderPlace = "Portal";
                           break;
                   }
               });
        }
    }
}
