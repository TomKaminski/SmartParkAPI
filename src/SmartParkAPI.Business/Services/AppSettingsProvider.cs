using System;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using SmartParkAPI.Contracts.DTO;
using SmartParkAPI.Contracts.DTO.Payments;
using SmartParkAPI.Contracts.Services;
using SmartParkAPI.Shared.Enums;

namespace SmartParkAPI.Business.Services
{
    public class AppSettingsProvider : IAppSettingsProvider
    {
        private readonly IHostingEnvironment _appEnv;
        private const string DefaultSmtpConfigurationKey = "Settings:SmtpConfiguration:";
        private const string PayuSettingsKey = "PayUSettings:";

        public AppSettingsProvider(IHostingEnvironment appEnv)
        {
            _appEnv = appEnv;
        }

        public IConfigurationRoot GetAppSettings(params AppSettingsType[] settings)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(_appEnv.ContentRootPath);

            foreach (var appSettingsType in settings)
            {
                builder.AddJsonFile($"{appSettingsType}.json");
            }

            return builder.Build();
        }

        public SmtpSettings GetSmtpSettings()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(_appEnv.ContentRootPath)
                .AddJsonFile($"{AppSettingsType.DefaultSettings}.json")
                .Build();

            return new SmtpSettings
            {
                SmtpDeliveryMethod = SmtpDeliveryMethod.Network,
                DeliveryFormat = SmtpDeliveryFormat.International,
                Host = configuration[$"{DefaultSmtpConfigurationKey}host"],
                Port = Convert.ToInt32(configuration[$"{DefaultSmtpConfigurationKey}port"]),
                UseDefaultCredentials =
                    Convert.ToBoolean(configuration[$"{DefaultSmtpConfigurationKey}defaultCredentials"]),
                EnableSsl = Convert.ToBoolean(configuration[$"{DefaultSmtpConfigurationKey}enableSsl"]),
                From = configuration[$"{DefaultSmtpConfigurationKey}from"],
                Credentials =
                    new NetworkCredential(configuration[$"{DefaultSmtpConfigurationKey}userName"],
                        configuration[$"{DefaultSmtpConfigurationKey}password"])
            };
        }

        public PaymentSettings GetPaymentSettings()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(_appEnv.WebRootPath)
                .AddJsonFile($"{AppSettingsType.DefaultSettings}.json")
                .Build();

            return new PaymentSettings
            {
                AuthorizeEndpoint = configuration[$"{PayuSettingsKey}AuthorizeEndpoint"],
                ClientSecondKey = configuration[$"{PayuSettingsKey}ClientSecondKey"],
                HostAddress = configuration[$"{PayuSettingsKey}HostAddress"],
                OAuthClientSecret = configuration[$"{PayuSettingsKey}OAuthClientSecret"],
                OrderCreateEndpoint = configuration[$"{PayuSettingsKey}OrderCreateEndpoint"],
                PosID = configuration[$"{PayuSettingsKey}PosID"]
            };
        }
    }
}
