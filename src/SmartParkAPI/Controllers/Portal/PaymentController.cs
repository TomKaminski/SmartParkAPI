using System;
using System.Security.Policy;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartParkAPI.Contracts.DTO.Payments;
using SmartParkAPI.Contracts.Services;
using SmartParkAPI.Contracts.Services.Payments;
using SmartParkAPI.Models.Base;
using SmartParkAPI.Models.Portal.Payment;
using SmartParkAPI.Shared.Enums;

namespace SmartParkAPI.Controllers.Portal
{
    [Area("Portal")]
    [Route("[area]/Payment")]
    [Authorize]
    public class PaymentController : BaseApiController
    {
        private readonly IPayuService _payuService;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public PaymentController(IPayuService payuService, IMapper mapper, IOrderService orderService)
        {
            _payuService = payuService;
            _mapper = mapper;
            _orderService = orderService;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> ProcessPayment([FromBody]PaymentRequestViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(SmartJsonResult<PaymentResponseViewModel>.Failure(GetModelStateErrors(ModelState)));

            // ReSharper disable once PossibleInvalidOperationException
            model.UserId = CurrentUser.UserId.Value;

            //var connection = HttpContext.Features.Get<IHttpConnectionFeature>();
            //model.CustomerIP = connection?.RemoteIpAddress?.ToString();ca

            model.CustomerIP = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1";
            model.UserEmail = CurrentUser.Email;
            model.UserName = CurrentUser.Name;
            model.UserLastName = CurrentUser.LastName;

            var request = _mapper.Map<PaymentRequest>(model);
            request.notifyUrl = Url.Action("Notify", "Payment", new { area = "Portal" }, "http");
            request.continueUrl = Url.Action("ShopContinue", "Home", new { area = "Portal" }, "http");
            var payuServiceResult = await _payuService.ProcessPaymentAsync(request, model.UserId, OrderPlace.Website);

            if (payuServiceResult.IsValid)
            {
                return Ok(SmartJsonResult<PaymentResponseViewModel>.Success(_mapper.Map<PaymentResponseViewModel>(payuServiceResult.Result)));
            }
            return BadRequest(SmartJsonResult<PaymentResponseViewModel>.Failure(payuServiceResult.ValidationErrors));
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("[action]")]
        public async Task<IActionResult> Notify([FromBody]PayuNotificationModel model)
        {
            await _orderService.UpdateOrderState(model.order.status, new Guid(model.order.extOrderId));
            return new EmptyResult();
        }
    }
}

