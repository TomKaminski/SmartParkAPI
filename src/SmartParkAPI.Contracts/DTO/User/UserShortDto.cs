using SmartParkAPI.Contracts.Common;

namespace SmartParkAPI.Contracts.DTO.User
{
    public class UserShortDto : BaseDto<int>
    {
        public string Name { get; set; }
        public string LastName { get; set; }
    }
}
