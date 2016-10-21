using System;
using SmartParkAPI.Shared.Enums;

namespace SmartParkAPI.Business.Providers
{
    public static class TokenValidityTimeProvider
    {
        public static DateTime? GetValidToDate(TokenType tokenType)
        {
            switch (tokenType)
            {
                case TokenType.ResetPasswordToken:
                    return DateTime.Now.AddDays(3);
                case TokenType.SelfDeleteToken:
                    return DateTime.Now.AddHours(8);
                case TokenType.ViewInBrowserToken:
                    return null;
                default:
                    return DateTime.Now;
            }
        }
    }
}
