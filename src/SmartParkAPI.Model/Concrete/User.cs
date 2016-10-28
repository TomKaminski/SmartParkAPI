using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using SmartParkAPI.Model.Common;

namespace SmartParkAPI.Model.Concrete
{
    public class User : Entity<int>
    {
        public User()
        {
            Orders = new HashSet<Order>();
            GateUsages = new HashSet<GateUsage>();
            UserPortalMessages = new HashSet<PortalMessage>();
            UserMessages = new HashSet<Message>();
        }

        public string Email { get; set; }
        public int Charges { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreateDate { get; set; }


        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public bool LockedOut { get; set; }
        public int UnsuccessfulLoginAttempts { get; set; }
        public DateTime? LockedTo { get; set; }

        public int UserPreferencesId { get; set; }
        public long? PasswordChangeTokenId { get; set; }

        public virtual Token PasswordChangeToken { get; set; }

        [ForeignKey("UserPreferencesId")]
        public virtual UserPreferences UserPreferences { get; set; }

        public virtual HashSet<UserDevice> UserDevices { get; set; }

        public virtual HashSet<Message> UserMessages { get; set; }
        public virtual HashSet<PortalMessage> UserPortalMessages { get; set; }

        public virtual HashSet<GateUsage> GateUsages { get; set; }
        public virtual HashSet<Order> Orders { get; set; }
    }
}
