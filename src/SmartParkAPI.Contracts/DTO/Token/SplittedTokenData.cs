using System;
using SmartParkAPI.Contracts.Common;
using SmartParkAPI.Shared.Enums;

namespace SmartParkAPI.Contracts.DTO.Token
{
    public class SplittedTokenData:BaseDto<long>
    {
        public DateTime? ValidTo { get; set; }
        public TokenType TokenType { get; set; }
        public Guid SecureToken { get; set; }

        public bool NotExpired()
        {
            return ValidTo == null || ValidTo > DateTime.Now;
        }
    }
}
