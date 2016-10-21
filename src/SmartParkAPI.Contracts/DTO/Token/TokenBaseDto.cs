using System;
using Newtonsoft.Json;
using SmartParkAPI.Contracts.Common;
using SmartParkAPI.Shared.Enums;
using SmartParkAPI.Shared.Helpers;

namespace SmartParkAPI.Contracts.DTO.Token
{
    public class TokenBaseDto : BaseDto<long>
    {
        public DateTime? ValidTo { get; set; }
        public TokenType TokenType { get; set; }
        public Guid SecureToken { get; set; }

        public string BuildEncryptedToken()
        {
            return EncryptHelper.Encrypt(JsonConvert.SerializeObject(this));
        }

        public bool NotExpired()
        {
            return ValidTo == null || ValidTo > DateTime.Now;
        }
    }
}
