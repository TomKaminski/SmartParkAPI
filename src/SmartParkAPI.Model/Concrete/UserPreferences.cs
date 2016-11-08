using System;
using SmartParkAPI.Model.Common;

namespace SmartParkAPI.Model.Concrete
{
    public class UserPreferences :Entity<int>
    {
        public bool ShrinkedSidebar { get; set; }
        public byte[] ProfilePhoto { get; set; }
        public Guid? ProfilePhotoId { get; set; }

        public int? UserId { get; set; }
        public virtual User User { get; set; }
    }
}
