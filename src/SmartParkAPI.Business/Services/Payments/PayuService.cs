using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SmartParkAPI.Contracts.Common;
using SmartParkAPI.Contracts.DTO.Order;
using SmartParkAPI.Contracts.DTO.Payments;
using SmartParkAPI.Contracts.Services;
using SmartParkAPI.Contracts.Services.Payments;
using SmartParkAPI.DataAccess.Interfaces;
using SmartParkAPI.Shared.Enums;

namespace SmartParkAPI.Business.Services.Payments
{
    public class PayuService : IPayuService
    {

        private readonly IPaymentAuthorizeService _paymentAuthorizeService;
        private readonly IPriceTresholdRepository _pricesRepository;
        private readonly IOrderService _orderService;
        private readonly PaymentSettings _paymentSettings;


        public PayuService(IPaymentAuthorizeService paymentAuthorizeService, IPriceTresholdRepository pricesRepository,
            IOrderService orderService, IAppSettingsProvider appSettingsProvider)
        {
            _paymentAuthorizeService = paymentAuthorizeService;
            _pricesRepository = pricesRepository;
            _orderService = orderService;
            _paymentSettings = appSettingsProvider.GetPaymentSettings();
        }

        public async Task<ServiceResult<PaymentResponse>> ProcessPaymentAsync(PaymentRequest request, int userId, OrderPlace orderPlace)
        {
            var authServiceResult = await _paymentAuthorizeService.GetAuthorizeTokenAsync();
            if (authServiceResult.IsValid)
            {
                using (var client = new HttpClient(new HttpClientHandler
                {
                    AllowAutoRedirect = false
                }))
                {
                    client.BaseAddress = new Uri(_paymentSettings.HostAddress);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                        authServiceResult.Result.access_token);

                    var orderPaymentInfo = await PrepareCompletePayuRequestAsync(request);
                    var requestBody = JsonConvert.SerializeObject(request);

                    var response = await client.PostAsync(_paymentSettings.OrderCreateEndpoint,
                                new StringContent(requestBody, Encoding.UTF8, "application/json"));

                    if (response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.Found)
                    {
                        var responseObj = JsonConvert.DeserializeObject<PaymentResponse>(await response.Content.ReadAsStringAsync());
                        await CreateNewOrderAsync(request, userId, orderPlace, orderPaymentInfo);
                        if (responseObj.status.statusCode == "SUCCESS")
                        {
                            return ServiceResult<PaymentResponse>.Success(responseObj);
                        }
                    }
                    return ServiceResult<PaymentResponse>.Failure(response.ReasonPhrase);
                }
            }
            return ServiceResult<PaymentResponse>.Failure(authServiceResult.ValidationErrors);
        }

        public async Task<ServiceResult<PaymentCardResponse>> ProcessCardPaymentAsync(PaymentCardRequest request, int userId, OrderPlace orderPlace)
        {
            var authServiceResult = await _paymentAuthorizeService.GetAuthorizeTokenAsync();
            if (authServiceResult.IsValid)
            {
                using (var client = new HttpClient(new HttpClientHandler
                {
                    AllowAutoRedirect = false
                }))
                {
                    client.BaseAddress = new Uri(_paymentSettings.HostAddress);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                        authServiceResult.Result.access_token);

                    var orderPaymentInfo = await PrepareCompleteCardPayuRequestAsync(request);
                    var requestBody = JsonConvert.SerializeObject(request);

                    //var response = await client.PostAsync(_paymentSettings.OrderCreateEndpoint,
                    //            new StringContent(requestBody, Encoding.UTF8, "application/json"));

                    //if (response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.Found)
                    //{
                    //    var responseObj = JsonConvert.DeserializeObject<PaymentCardResponse>(await response.Content.ReadAsStringAsync());
                    //    await CreateNewOrderAsync(request, userId, orderPlace, orderPaymentInfo);
                    //    if (responseObj.status.statusCode == "SUCCESS")
                    //    {
                    //        return ServiceResult<PaymentCardResponse>.Success(responseObj);
                    //    }
                    //}
                    //return ServiceResult<PaymentCardResponse>.Failure(response.ReasonPhrase);


                    //TODO:Uncomment above code when payu express integrtation will work
                    await CreateNewOrderAsync(request, userId, orderPlace, orderPaymentInfo);
                    return ServiceResult<PaymentCardResponse>.Success(new PaymentCardResponse
                    {
                        status = new CardStatus
                        {
                            statusCode = new Random().Next(0,2) >= 1 ? "SUCCESS" : "ERROR",
                            statusDesc = "Request successful"
                        },
                        orderId = Guid.NewGuid().ToString(),
                        payMethods = new CardPayMethods
                        {
                            payMethod = new CardPayMethod
                            {
                                value = request.payMethods.payMethod.value,
                                card = new Card
                                {
                                    expirationMonth = "12",
                                    expirationYear = "2018",
                                    number = "12345*****2314"
                                }
                            }
                        }
                    });
                }
            }
            return ServiceResult<PaymentCardResponse>.Failure(authServiceResult.ValidationErrors);
        }

        private async Task<OrderPaymentInfo> PrepareCompletePayuRequestAsync(PaymentRequest request)
        {
            var product = request.products.First();

            var priceTreshold =
                (await _pricesRepository.GetAllAsync(x => x.MinCharges <= Convert.ToInt32(product.quantity) && !x.IsDeleted))
                    .OrderByDescending(x => x.MinCharges).First();

            var totalPrice = (priceTreshold.PricePerCharge * Convert.ToInt32(product.quantity));

            product.unitPrice = (priceTreshold.PricePerCharge * 100).ToString("####");
            request.extOrderId = (await _orderService.GenerateExternalOrderIdAsync()).Result.ToString();
            request.totalAmount = (totalPrice * 100).ToString("####");
            request.merchantPosId = _paymentSettings.PosID;

            return new OrderPaymentInfo
            {
                TotalAmount = totalPrice,
                PricePerCharge = priceTreshold.PricePerCharge,
                PriceTresholdId = priceTreshold.Id
            };
        }

        private async Task<OrderPaymentInfo> PrepareCompleteCardPayuRequestAsync(PaymentCardRequest request)
        {
            var product = request.products.First();

            var priceTreshold =
                (await _pricesRepository.GetAllAsync(x => x.MinCharges <= Convert.ToInt32(product.quantity) && !x.IsDeleted))
                    .OrderByDescending(x => x.MinCharges).First();

            var totalPrice = (priceTreshold.PricePerCharge * Convert.ToInt32(product.quantity));

            product.unitPrice = (priceTreshold.PricePerCharge * 100).ToString("####");
            request.extOrderId = (await _orderService.GenerateExternalOrderIdAsync()).Result.ToString();
            request.totalAmount = (totalPrice * 100).ToString("####");
            request.merchantPosId = _paymentSettings.PosID;

            return new OrderPaymentInfo
            {
                TotalAmount = totalPrice,
                PricePerCharge = priceTreshold.PricePerCharge,
                PriceTresholdId = priceTreshold.Id
            };
        }

        private async Task CreateNewOrderAsync(PaymentRequest request, int userId, OrderPlace orderPlace, OrderPaymentInfo paymentInfo)
        {
            var order = new OrderBaseDto
            {
                UserId = userId,
                Date = DateTime.Now,
                ExtOrderId = new Guid(request.extOrderId),
                NumOfCharges = Convert.ToInt32(request.products[0].quantity),
                OrderPlace = orderPlace,
                OrderState = OrderStatus.Pending,
                PricePerCharge = paymentInfo.PricePerCharge,
                PriceTresholdId = paymentInfo.PriceTresholdId,
                Price = paymentInfo.TotalAmount,
            };

            await _orderService.CreateAsync(order);
        }

        private async Task CreateNewOrderAsync(PaymentCardRequest request, int userId, OrderPlace orderPlace, OrderPaymentInfo paymentInfo)
        {
            var order = new OrderBaseDto
            {
                UserId = userId,
                Date = DateTime.Now,
                ExtOrderId = new Guid(request.extOrderId),
                NumOfCharges = Convert.ToInt32(request.products[0].quantity),
                OrderPlace = orderPlace,
                OrderState = OrderStatus.Completed, //TODO: CHANGE TO PENDING WHEN PAYU EXPRESS INTEGRATION WILL WORK
                PricePerCharge = paymentInfo.PricePerCharge,
                PriceTresholdId = paymentInfo.PriceTresholdId,
                Price = paymentInfo.TotalAmount,
            };

            await _orderService.CreateAsync(order);
        }
    }
}
