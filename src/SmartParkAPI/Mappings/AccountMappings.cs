using AutoMapper;
using SmartParkAPI.Contracts.DTO.User;
using SmartParkAPI.Models;
using SmartParkAPI.Models.Account;
using SmartParkAPI.Models.Portal.Account;
using SmartParkAPI.Models.Portal.Manage;
using SmartParkAPI.Shared.Helpers;

namespace SmartParkAPI.Mappings
{
    public class AccountFrontendMappings : Profile
    {
        protected override void Configure()
        {
            CreateMap<RegisterViewModel, UserBaseDto>()
              .ForMember(x => x.Charges, opt => opt.UseValue(0))
              .ForMember(x => x.IsAdmin, opt => opt.UseValue(false))
              .ForMember(x => x.LockedOut, opt => opt.UseValue(false))
              .ForMember(x => x.UnsuccessfulLoginAttempts, opt => opt.UseValue(0))
              .IgnoreNotExistingProperties();

            CreateMap<LoginViewModel, UserBaseDto>().IgnoreNotExistingProperties();

            CreateMap<UserBaseDto, AppUserState>().IgnoreNotExistingProperties();

            CreateMap<ChangeUserInfoViewModel, UserBaseDto>().IgnoreNotExistingProperties();
            CreateMap<UserBaseDto, ChangeUserInfoViewModel>().IgnoreNotExistingProperties();
            CreateMap<UserBaseDto, GetUserApiModel>().IgnoreNotExistingProperties();

        }
    }
}
