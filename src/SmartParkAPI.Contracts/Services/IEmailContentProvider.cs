using System.Collections.Generic;
using SmartParkAPI.Contracts.Services.Base;
using SmartParkAPI.Shared.Enums;

namespace SmartParkAPI.Contracts.Services
{
    public interface IEmailContentProvider : IDependencyService
    {
        string GetEmailBody(EmailType type, Dictionary<string, string> parameters);
        string GetLayoutTemplate();
        string GetValidTemplateString(EmailType type);
        string GetEmailTitle(EmailType type);
    }
}