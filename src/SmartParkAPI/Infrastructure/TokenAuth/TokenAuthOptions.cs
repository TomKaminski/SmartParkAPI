using System;
using Microsoft.IdentityModel.Tokens;

namespace SmartParkAPI.Infrastructure.TokenAuth
{
    [Obsolete("We are not using JW Tokens for now.")]
    public class TokenAuthOptions
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public SigningCredentials SigningCredentials { get; set; }
    }
}
