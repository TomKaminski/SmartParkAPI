using System;
using SmartParkAPI.Model.Common;
using SmartParkAPI.Shared.Enums;

namespace SmartParkAPI.Model.Concrete
{
    public class Token:Entity<long>
    {
        public DateTime? ValidTo { get; set; }
        public TokenType TokenType { get; set; }
        public Guid SecureToken { get; set; }
        public DateTime CreatedOn { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
