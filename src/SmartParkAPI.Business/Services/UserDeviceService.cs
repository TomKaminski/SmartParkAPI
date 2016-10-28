using System;
using System.Threading.Tasks;
using AutoMapper;
using SmartParkAPI.Business.Services.Base;
using SmartParkAPI.Contracts.Common;
using SmartParkAPI.Contracts.DTO.User;
using SmartParkAPI.Contracts.DTO.UserDevice;
using SmartParkAPI.Contracts.Services;
using SmartParkAPI.DataAccess.Common;
using SmartParkAPI.DataAccess.Interfaces;
using SmartParkAPI.Model.Concrete;

namespace SmartParkAPI.Business.Services
{
    public class UserDeviceService : EntityService<UserDeviceDto, UserDevice, int>, IUserDeviceService
    {
        private readonly IUserDeviceRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UserDeviceService(IUserDeviceRepository repository, IUnitOfWork unitOfWork, IMapper mapper, IMapper mapper1, IUnitOfWork unitOfWork1) : base(repository, unitOfWork, mapper)
        {
            _repository = repository;
            _mapper = mapper1;
            _unitOfWork = unitOfWork1;
        }

        public async Task<ServiceResult<UserDeviceDto>> CreateUpdateMobileTokenAsync(UserBaseDto userDto, string deviceName)
        {
            var currentUserDeviceEntry = await _repository.FirstOrDefaultAsync(x => x.UserId == userDto.Id && x.Name == deviceName);
            if (currentUserDeviceEntry != null)
            {
                _repository.Delete(_mapper.Map<UserDevice>(currentUserDeviceEntry));
            }

            var newUserDeviceEntry = new UserDevice
            {
                UserId = userDto.Id,
                Name = deviceName,
                Token = Guid.NewGuid().ToString(),
                CanAccess = true
            };

            var ud = _repository.Add(newUserDeviceEntry);
            await _unitOfWork.CommitAsync();
            return ServiceResult<UserDeviceDto>.Success(_mapper.Map<UserDeviceDto>(ud));
        }
    }
}
