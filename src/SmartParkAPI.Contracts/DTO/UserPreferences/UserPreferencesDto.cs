using System;
using SmartParkAPI.Contracts.Common;

namespace SmartParkAPI.Contracts.DTO.UserPreferences
{
    public class UserPreferencesDto : BaseDto<int>
    {
        public bool ShrinkedSidebar { get; set; }
        public byte[] ProfilePhoto { get; set; }
        public Guid? ProfilePhotoId { get; set; }

        public int UserId { get; set; }
    }
}
