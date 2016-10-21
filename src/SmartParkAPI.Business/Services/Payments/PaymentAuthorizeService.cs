using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SmartParkAPI.Contracts.Common;
using SmartParkAPI.Contracts.DTO.Payments;
using SmartParkAPI.Contracts.Services;
using SmartParkAPI.Contracts.Services.Payments;

namespace SmartParkAPI.Business.Services.Payments
{
    public class PaymentAuthorizeService : IPaymentAuthorizeService
    {

        private readonly PaymentSettings _paymentSettings;

        public PaymentAuthorizeService(IAppSettingsProvider appSettingsProvider)
        {
            _paymentSettings = appSettingsProvider.GetPaymentSettings();
        }


        public async Task<ServiceResult<PaymentAuthorizationResponse>> GetAuthorizeTokenAsync()
        {
            using (var client = new HttpClient(new HttpClientHandler
            {
                AllowAutoRedirect = false
            }))
            {
                client.BaseAddress = new Uri(_paymentSettings.HostAddress);
                var postData = GetRequestBodyForAuthorization();

                HttpContent requestBody = new FormUrlEncodedContent(postData);

                var response = await client.PostAsync(_paymentSettings.AuthorizeEndpoint, requestBody);

                if (response.IsSuccessStatusCode)
                {
                    var stringResult = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<PaymentAuthorizationResponse>(stringResult);
                    return ServiceResult<PaymentAuthorizationResponse>.Success(result);
                }
                return ServiceResult<PaymentAuthorizationResponse>.Failure(response.ReasonPhrase);
            }
        }

        private IEnumerable<KeyValuePair<string, string>> GetRequestBodyForAuthorization()
        {
            return new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("grant_type", "client_credentials"),
                    new KeyValuePair<string, string>("client_id", _paymentSettings.PosID),
                    new KeyValuePair<string, string>("client_secret", _paymentSettings.OAuthClientSecret)
                };
        }
    }
}
