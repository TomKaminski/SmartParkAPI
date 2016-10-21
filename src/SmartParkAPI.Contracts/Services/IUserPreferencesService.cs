using System;
using System.Threading.Tasks;
using SmartParkAPI.Contracts.Common;
using SmartParkAPI.Contracts.DTO.UserPreferences;
using SmartParkAPI.Contracts.Services.Base;

namespace SmartParkAPI.Contracts.Services
{
    public interface IUserPreferencesService : IEntityService<UserPreferencesDto, int>, IDependencyService
    {
        Task<ServiceResult<Guid>> SetUserAvatarAsync(byte[] sourceImage, int userId, string folderPath);
        Task<ServiceResult<string>> DeleteProfilePhotoAsync(int userId, string folderPath);

        Task<ServiceResult<UserPreferencesDto>> SaveChartPreferenceAsync(UserPreferenceChartSettingsDto userPreferenceChartDto);
    }
}
