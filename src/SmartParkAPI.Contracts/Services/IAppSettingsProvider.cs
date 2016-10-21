using Microsoft.Extensions.Configuration;
using SmartParkAPI.Contracts.DTO;
using SmartParkAPI.Contracts.DTO.Payments;
using SmartParkAPI.Contracts.Services.Base;
using SmartParkAPI.Shared.Enums;

namespace SmartParkAPI.Contracts.Services
{
    public interface IAppSettingsProvider : IDependencyService
    {
        IConfigurationRoot GetAppSettings(params AppSettingsType[] settings);
        SmtpSettings GetSmtpSettings();
        PaymentSettings GetPaymentSettings();
    }
}
