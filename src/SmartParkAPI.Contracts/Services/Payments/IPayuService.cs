using System.Threading.Tasks;
using SmartParkAPI.Contracts.Common;
using SmartParkAPI.Contracts.DTO.Payments;
using SmartParkAPI.Contracts.Services.Base;
using SmartParkAPI.Shared.Enums;

namespace SmartParkAPI.Contracts.Services.Payments
{
    public interface IPayuService : IDependencyService
    {
        Task<ServiceResult<PaymentResponse>> ProcessPaymentAsync(PaymentRequest request, int userId, OrderPlace orderPlace);
        Task<ServiceResult<PaymentCardResponse>> ProcessCardPaymentAsync(PaymentCardRequest request, int userId, OrderPlace orderPlace);

    }
}
