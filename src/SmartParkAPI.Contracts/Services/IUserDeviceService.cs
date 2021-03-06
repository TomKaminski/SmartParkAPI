﻿using System.Threading.Tasks;
using SmartParkAPI.Contracts.Common;
using SmartParkAPI.Contracts.DTO.User;
using SmartParkAPI.Contracts.DTO.UserDevice;
using SmartParkAPI.Contracts.DTO.UserPreferences;
using SmartParkAPI.Contracts.Services.Base;

namespace SmartParkAPI.Contracts.Services
{
    public interface IUserDeviceService : IEntityService<UserDeviceDto, int>, IDependencyService
    {
        Task<ServiceResult<UserDeviceDto>> CreateUpdateMobileTokenAsync(UserBaseDto userDto, string deviceName);
        Task<ServiceResult<UserBaseDto, UserPreferencesDto>> ValidateMobileTokenAsync(string email, string token);
    }
}
