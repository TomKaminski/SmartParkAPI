using System;
using System.Collections.Generic;
using System.IO;
using SmartParkAPI.Contracts.Services;
using SmartParkAPI.Shared.Enums;
using Microsoft.AspNetCore.Hosting;

namespace SmartParkAPI.Business.Providers.Email
{
    public class EmailContentProvider : IEmailContentProvider
    {
        private readonly IHostingEnvironment _appEnv;
        private readonly IAppSettingsProvider _appSettingsProvider;
        private const string EmailsBasePath = "/Content/Emails";
        private const string BodyMarker = "{{BodyHtml}}";
        public EmailContentProvider()
        {

        }
        public EmailContentProvider(IHostingEnvironment appEnv, IAppSettingsProvider appSettingsProvider)
        {
            _appEnv = appEnv;
            _appSettingsProvider = appSettingsProvider;
        }
        public virtual string GetEmailBody(EmailType type, Dictionary<string, string> parameters)
        {
            var validTemplate = GetValidTemplateString(type);
            var templateWithLayout = InsertBodyIntoLayout(validTemplate);
            return PrepareEmailBody(templateWithLayout, parameters);
        }
        public virtual string GetValidTemplateString(EmailType type)
        {
            switch (type)
            {
                case EmailType.Register: return File.ReadAllText($"{_appEnv.ContentRootPath}{EmailsBasePath}/Register.html");
                case EmailType.ResetPassword: return File.ReadAllText($"{_appEnv.ContentRootPath}{EmailsBasePath}/ResetPassword.html");
                case EmailType.SelfDelete: return File.ReadAllText($"{_appEnv.ContentRootPath}{EmailsBasePath}/SelfDelete.html");

            }
            return "";
        }
        public virtual string GetEmailTitle(EmailType type)
        {
            var conf = _appSettingsProvider.GetAppSettings(AppSettingsType.Resources);
            switch (type)
            {
                case EmailType.Register:  return conf["EmailResources:RegisterEmail_Title"];
                case EmailType.ResetPassword:  return conf["EmailResources:ResetPassword_Title"];
                case EmailType.SelfDelete: return conf["EmailResources:SelfDelete_Title"];
            }
            throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        public virtual string GetLayoutTemplate()
        {
            return File.ReadAllText(GetLayoutPath());
        }

        private string PrepareEmailBody(string template, Dictionary<string, string> parameters)
        {
            var localTemplate = template;
            foreach (var parameter in parameters)
            {
                localTemplate = localTemplate.Replace("{{" + parameter.Key + "}}", parameter.Value);
            }
            return localTemplate;
        }
        private string GetLayoutPath()
        {
            return $"{_appEnv.ContentRootPath}{EmailsBasePath}/_EmailLayout.html";
        }
        private string InsertBodyIntoLayout(string bodyHtml)
        {
            return GetLayoutTemplate().Replace(BodyMarker, bodyHtml);
        }
    }
}
