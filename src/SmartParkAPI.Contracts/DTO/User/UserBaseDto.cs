using System;

namespace SmartParkAPI.Contracts.DTO.User
{
    public class UserBaseDto : UserShortDto
    {
        public string Email { get; set; }
        public int Charges { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }


        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public bool LockedOut { get; set; }
        public int UnsuccessfulLoginAttempts { get; set; }
        public DateTime? LockedTo { get; set; }


        public long? PasswordChangeTokenId { get; set; }
        public long? EmailChangeTokenId { get; set; }
    }
}
