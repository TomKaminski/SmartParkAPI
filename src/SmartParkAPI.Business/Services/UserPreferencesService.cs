using System;
using System.Threading.Tasks;
using AutoMapper;
using SmartParkAPI.Business.Services.Base;
using SmartParkAPI.Contracts.Common;
using SmartParkAPI.Contracts.DTO.UserPreferences;
using SmartParkAPI.Contracts.Services;
using SmartParkAPI.DataAccess.Common;
using SmartParkAPI.DataAccess.Interfaces;
using SmartParkAPI.Model.Concrete;

namespace SmartParkAPI.Business.Services
{
    public class UserPreferenesService:EntityService<UserPreferencesDto,UserPreferences, int>, IUserPreferencesService
    {
        private readonly IUserPreferencesRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageProcessorService _imageProcessorService;
        private readonly IMapper _mapper;
        private const string PlaceholderPhotoName = "avatar-placeholder";

        public UserPreferenesService(IUserPreferencesRepository repository, IUnitOfWork unitOfWork, IMapper mapper, IImageProcessorService imageProcessorService, IMapper mapper1) : base(repository, unitOfWork, mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _imageProcessorService = imageProcessorService;
            _mapper = mapper1;
        }

        public async Task<ServiceResult<Guid>> SetUserAvatarAsync(byte[] sourceImage, int userId, string folderPath)
        {
            var imageProcessorJob = _imageProcessorService.ProcessAndSaveImage(sourceImage, folderPath);
            var userPreference = await _repository.SingleOrDefaultAsync(x => x.UserId == userId);
            if (userPreference.ProfilePhotoId != null)
            {
                _imageProcessorService.DeleteImagesByPath(folderPath+userPreference.ProfilePhotoId);
            }
            userPreference.ProfilePhoto = imageProcessorJob.Result;
            userPreference.ProfilePhotoId = imageProcessorJob.SecondResult;
            _repository.Edit(userPreference);
            await _unitOfWork.CommitAsync();
            return ServiceResult<Guid>.Success(imageProcessorJob.SecondResult);
        }

        public async Task<ServiceResult<string>> DeleteProfilePhotoAsync(int userId, string folderPath)
        {
            var userPreference = await _repository.SingleOrDefaultAsync(x => x.UserId == userId);
            _imageProcessorService.DeleteImagesByPath(folderPath + userPreference.ProfilePhotoId);

            userPreference.ProfilePhoto = null;
            userPreference.ProfilePhotoId = null;
            _repository.Edit(userPreference);
            await _unitOfWork.CommitAsync();
            return ServiceResult<string>.Success(PlaceholderPhotoName);
        }

        public async Task<ServiceResult<UserPreferencesDto>> SaveChartPreferenceAsync(UserPreferenceChartSettingsDto userPreferenceChartDto)
        {
            var userPreference = await _repository.SingleOrDefaultAsync(x => x.UserId == userPreferenceChartDto.UserId);
            _mapper.Map(userPreferenceChartDto, userPreference);
            _repository.Edit(userPreference);
            await _unitOfWork.CommitAsync();
            return ServiceResult<UserPreferencesDto>.Success(_mapper.Map<UserPreferencesDto>(userPreference));
        }
    }
}
