using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SmartParkAPI.Contracts.DTO;
using SmartParkAPI.Contracts.DTO.Chart;
using SmartParkAPI.Contracts.DTO.GateUsage;
using SmartParkAPI.Contracts.DTO.Order;
using SmartParkAPI.Contracts.DTO.Payments;
using SmartParkAPI.Contracts.DTO.PortalMessage;
using SmartParkAPI.Contracts.DTO.PriceTreshold;
using SmartParkAPI.Contracts.DTO.User;
using SmartParkAPI.Contracts.DTO.UserPreferences;
using SmartParkAPI.Contracts.DTO.Weather;
using SmartParkAPI.Contracts.DTO.WeatherInfo;
using SmartParkAPI.Models;
using SmartParkAPI.Models.Panel;
using SmartParkAPI.Models.Portal.Chart;
using SmartParkAPI.Models.Portal.GateUsage;
using SmartParkAPI.Models.Portal.Message;
using SmartParkAPI.Models.Portal.Payment;
using SmartParkAPI.Models.Portal.PortalMessage;
using SmartParkAPI.Models.Portal.PriceTreshold;
using SmartParkAPI.Models.Portal.User;
using SmartParkAPI.Models.Portal.Weather;
using SmartParkAPI.Shared.Enums;
using SmartParkAPI.Shared.Helpers;

namespace SmartParkAPI.Mappings
{
    public class FrontendMappings : Profile
    {
        public FrontendMappings()
        {
            CreateMap<ParkingAthMessage, MessageDto>().IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<MessageDto, ParkingAthMessage>();

            CreateMap<UserBaseDto, UserBaseViewModel>()
                .ForMember(x => x.CreateDate, opt => opt.MapFrom(src => src.CreateDate.ToLongDateString()))
                .ForMember(x => x.Range, src => src.MapFrom(y => y.IsAdmin ? "Administrator" : "Użytkownik"))
                ;

            CreateMap<WeatherDto, WeatherDataViewModel>()
                .ForMember(x => x.DateOfRead, opt => opt.MapFrom(src => src.DateOfRead.ToString("dd MMMM yyyy")))
                .ForMember(x => x.HourOfRead, opt => opt.MapFrom(src => src.DateOfRead.Hour))
                ;

            CreateMap<WeatherInfoDto, WeatherInfoDataViewModel>()
                .ForMember(x => x.WeatherId, src => src.MapFrom(a => a.WeatherConditionId))
                ;

            CreateMap<QuickMessageViewModel, PortalMessageDto>()
                .ForMember(x => x.PortalMessageType, opt => opt.UseValue(PortalMessageEnum.MessageToAdminFromUser))
                ;

            CreateMap<ReplyMessageViewModel, PortalMessageDto>();

            CreateMap<ChartDataRequest, ChartRequestDto>()
                .ForMember(x => x.DateRange, opt => opt.MapFrom(x => new DateRange(x.StartDate, x.EndDate)))
                ;

            CreateMap<ChartDataRequest, UserPreferenceChartSettingsDto>();

            CreateMap<ChartRequestDto, ChartPreferencesReturnModel>()
                .ForMember(x => x.StartDate, opt => opt.MapFrom(a => a.DateRange.StartDate))
                .ForMember(x => x.EndDate, opt => opt.MapFrom(a => a.DateRange.EndDate))
                .ForMember(x => x.LabelStartDate, opt => opt.MapFrom(a => a.DateRange.StartDate.ToString("dd-MM-yy")))
                .ForMember(x => x.LabelEndDate, opt => opt.MapFrom(a => a.DateRange.EndDate.ToString("dd-MM-yy")))
                ;


            CreateMap<ChartListDto, ChartDataReturnModel>()
                .ForMember(dest => dest.Labels, opt => opt.MapFrom(x => x.Elements.Select(el => el.DateLabel).ToArray()))
                .ForMember(dest => dest.Data, opt => opt.MapFrom(x => x.Elements.Select(el => el.NodeValue).ToArray()))
                ;

            CreateMap<PortalMessageDto, PortalMessageItemViewModel>()
                .ForMember(x => x.CreateDate, opt => opt.MapFrom(a => a.CreateDate.ToString("dd.MM.yy HH:mm")))
                ;

            CreateMap<PortalMessageUserDto, PortalMessageUserViewModel>()
                .ForMember(x => x.IsDeleted, opt => opt.MapFrom(a => a.Id == 0))
                ;

            CreateMap<PortalMessageClusterDto, PortalMessageClusterViewModel>()
                .ForMember(x => x.ReceiverUser, opt => opt.MapFrom(a => a.ReceiverUser))
                .ForMember(x => x.Messages, opt => opt.MapFrom(a => a.Cluster))
                ;

            CreateMap<PortalMessageClustersDto, PortalMessageClustersViewModel>()
                .ForMember(x => x.User, opt => opt.MapFrom(a => a.User))
                .ForMember(x => x.Clusters, opt => opt.MapFrom(a => a.Clusters))
                ;

            CreateMap<PriceTresholdBaseDto, PriceTresholdShopItemViewModel>()
                .ForMember(x => x.PriceLabel, a => a.MapFrom(s => s.PricePerCharge.ToString("#.00")))
                ;

            CreateMap<OrderBaseDto, ShopOrderItemViewModel>()
                .ForMember(x => x.Price, a => a.MapFrom(s => s.Price.ToString("#.00")))
                .ForMember(x => x.PricePerCharge, a => a.MapFrom(s => s.PricePerCharge.ToString("#.00")))
                .ForMember(x => x.Date, a => a.MapFrom(s => s.Date.ToString("dd.MM.yyyy")))
                .ForMember(x => x.Time, a => a.MapFrom(s => s.Date.ToString("HH:mm")))
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

            CreateMap<PaymentRequestViewModel, PaymentRequest>()
                .ForMember(x => x.buyer, a => a.MapFrom(s => new Contracts.DTO.Payments.Buyer
                {
                    email = s.UserEmail,
                    firstName = s.UserName,
                    lastName = s.UserLastName
                }))
                .ForMember(x => x.description, a => a.MapFrom(s => $"Zakup wyjazdów w SmartPark - {s.Charges}x"))
                .ForMember(x => x.products, s => s.MapFrom(a => new List<Contracts.DTO.Payments.Product>
                {
                    new Contracts.DTO.Payments.Product
                    {
                        name = $"SmartPark - wyjazdy - {a.Charges}x",
                        quantity = a.Charges.ToString(),
                    }
                }))
                .ForMember(x => x.customerIp, a => a.MapFrom(s => s.CustomerIP));


            CreateMap<PaymentRequestApiModel, PaymentCardRequest>()
                .ForMember(x => x.buyer, a => a.MapFrom(s => new Contracts.DTO.Payments.Buyer
                {
                    email = s.UserEmail,
                    firstName = s.UserName,
                    lastName = s.UserLastName
                }))
                .ForMember(x => x.payMethods, s => s.MapFrom(a => new PayMethods
                {
                    payMethod = new Contracts.DTO.Payments.PayMethod
                    {
                        type = "CARD_TOKEN",
                        value = a.CardTokenValue
                    }
                }))
                .ForMember(x => x.deviceFingerprint, a => a.MapFrom(s => s.DeviceFingerPrint))
                .ForMember(x => x.description, a => a.MapFrom(s => $"Zakup wyjazdów w SmartPark - {s.Charges}x"))
                .ForMember(x => x.products, s => s.MapFrom(a => new List<Contracts.DTO.Payments.Product>
                {
                    new Contracts.DTO.Payments.Product
                    {
                        name = $"SmartPark - wyjazdy - {a.Charges}x",
                        quantity = a.Charges.ToString(),
                    }
                }))
                .ForMember(x => x.customerIp, a => a.MapFrom(s => s.CustomerIP));

            CreateMap<PaymentResponse, PaymentResponseViewModel>()
                .ForMember(x => x.RedirectUri, a => a.MapFrom(s => s.redirectUri));


            //CreateMap<PayuNotificationModel, PaymentNotification>();

            CreateMap<GateUsageBaseDto, GateOpeningViewModel>()
                .ForMember(x => x.Date, s => s.MapFrom(a => a.DateOfUse.ToString("dd MMMM yyyy")))
                .ForMember(x => x.Time, s => s.MapFrom(a => a.DateOfUse.ToString("HH:mm")))
                ;
        }
    }
}
