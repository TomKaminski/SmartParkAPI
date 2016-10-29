using SmartParkAPI.Contracts.Common;

namespace SmartParkAPI.Contracts.DTO.UserDevice
{
    public class UserDeviceDto : BaseDto<int>
    {
        public string Name { get; set; }
        public string Token { get; set; }

        public int UserId { get; set; }
    }
}
