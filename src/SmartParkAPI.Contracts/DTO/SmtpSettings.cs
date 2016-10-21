using System.Net;
using System.Net.Mail;

namespace SmartParkAPI.Contracts.DTO
{
    public class SmtpSettings
    {
        public SmtpDeliveryFormat DeliveryFormat { get; set; }
        public SmtpDeliveryMethod SmtpDeliveryMethod { get; set; }
        public bool EnableSsl { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public ICredentialsByHost Credentials { get; set; }
        public string From { get; set; }
    }
}
