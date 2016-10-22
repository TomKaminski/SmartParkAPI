using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SmartParkAPI.Infrastructure.Extensions
{
    public static class AntiForgeryTokenExtensions
    {
        public static string RequestVerificationToken(this IHtmlHelper helper)
        {
            var antiforgeryInputHtmlString = helper.AntiForgeryToken().ToString();
            var startindex = antiforgeryInputHtmlString.IndexOf("value=", StringComparison.Ordinal) + 7;

            var endOfToken = antiforgeryInputHtmlString.IndexOf("\" />", StringComparison.Ordinal);
            var endLength = 4;
            if (endOfToken == 0)
            {
                endLength = 3;
            }

            var tokenvalue = antiforgeryInputHtmlString.Substring(startindex, antiforgeryInputHtmlString.Length - startindex - endLength);
            return tokenvalue;
        }
    }

    
}
