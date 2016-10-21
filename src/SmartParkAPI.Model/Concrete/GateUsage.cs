using System;
using SmartParkAPI.Model.Common;

namespace SmartParkAPI.Model.Concrete
{
    public class GateUsage : Entity<Guid>
    {
        public DateTime DateOfUse { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
