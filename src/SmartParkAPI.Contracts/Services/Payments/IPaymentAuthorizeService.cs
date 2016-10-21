using System.Threading.Tasks;
using SmartParkAPI.Contracts.Common;
using SmartParkAPI.Contracts.DTO.Payments;
using SmartParkAPI.Contracts.Services.Base;

namespace SmartParkAPI.Contracts.Services.Payments
{
    public interface IPaymentAuthorizeService : IDependencyService
    {
        Task<ServiceResult<PaymentAuthorizationResponse>> GetAuthorizeTokenAsync();
    }
}
