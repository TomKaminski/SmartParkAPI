using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartParkAPI.Contracts.DTO.User;
using SmartParkAPI.Contracts.Services;
using SmartParkAPI.Models.Account;
using SmartParkAPI.Models.Base;
using SmartParkAPI.Models.Panel;
using SmartParkAPI.Models.Portal.PriceTreshold;
using SmartParkAPI.Shared.Enums;

namespace SmartParkAPI.Controllers
{
    [Route("api/Account")]
    public class AccountApiController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly IMessageService _messageService;
        private readonly IMapper _mapper;
        private readonly IPriceTresholdService _priceTresholdService;

        public AccountApiController(IUserService userService, IMessageService messageService, IMapper mapper, IPriceTresholdService priceTresholdService)
        {
            _userService = userService;
            _messageService = messageService;
            _mapper = mapper;
            _priceTresholdService = priceTresholdService;
        }

        [HttpPost]
        [Route("Forgot")]
        public async Task<SmartJsonResult<bool>> ForgotPassword([FromBody] ForgotApiModel model)
        {
            if (!ModelState.IsValid)
                return SmartJsonResult<bool>.Failure(GetModelStateErrors(ModelState));

            var changePasswordTokenResult = await _userService.GetPasswordChangeTokenAsync(model.Email);
            var changePasswordUrl = $"{Url.Action("RedirectFromToken", "Token", null, "http")}?id={changePasswordTokenResult.SecondResult}";
            await _messageService.SendMessageAsync(EmailType.ResetPassword, changePasswordTokenResult.Result, GetAppBaseUrl(),
                new Dictionary<string, string> { { "ChangePasswordLink", changePasswordUrl } });

            return changePasswordTokenResult.IsValid
                ? SmartJsonResult<bool>.Success(true)
                : SmartJsonResult<bool>.Failure(changePasswordTokenResult.ValidationErrors);
        }

        [HttpPost]
        [Route("CheckAccount")]
        public async Task<SmartJsonResult<GetUserApiModel>> CheckAccount([FromBody] CheckAccountApiModel model)
        {
            if (!ModelState.IsValid)
                return SmartJsonResult<GetUserApiModel>.Failure(GetModelStateErrors(ModelState));

            var checkAccountExistResult = await _userService.GetByEmailAsync(model.Email);

            return checkAccountExistResult.IsValid
                ? SmartJsonResult<GetUserApiModel>.Success(_mapper.Map<GetUserApiModel>(checkAccountExistResult.Result))
                : SmartJsonResult<GetUserApiModel>.Failure(checkAccountExistResult.ValidationErrors);
        }


        [HttpPost]
        [Route("GetPrices")]
        public async Task<IActionResult> GetPrices()
        {
            var currentPricesResult = await _priceTresholdService.GetAllAsync();
            if (currentPricesResult.IsValid)
            {
                var pricesViewModels = currentPricesResult.Result.Select(_mapper.Map<PriceTresholdShopItemViewModel>).ToList();
                var defaultPrice = pricesViewModels.First();
                defaultPrice.IsDeafult = true;

                for (var i = 1; i < pricesViewModels.Count; i++)
                {
                    var price = pricesViewModels[i];
                    price.PercentDiscount = 100 - Convert.ToInt32((price.PricePerCharge * 100) / defaultPrice.PricePerCharge);
                }

                return Json(SmartJsonResult<IEnumerable<PriceTresholdShopItemViewModel>>.Success(pricesViewModels));
            }
            return Json(SmartJsonResult<IEnumerable<PriceTresholdShopItemViewModel>>.Failure(currentPricesResult.ValidationErrors));
        }

        #region TokenAuth - OBSOLETE
        ///// <summary>
        ///// Request a new token for a given username/password pair.
        ///// </summary>
        ///// <param name="req"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[AllowAnonymous]
        //[Route("Login")]
        //public async Task<dynamic> Login([FromBody] AuthRequest req)
        //{
        //    var loginApiResult = await _userService.LoginAsync(req.Username, req.Password);
        //    // Obviously, at this point you need to validate the username and password against whatever system you wish.
        //    if (loginApiResult.IsValid)
        //    {
        //        DateTime? expires = DateTime.UtcNow.AddMinutes(10);
        //        var token = await GetTokenAsync(req.Username, expires);
        //        return new { authenticated = true, entityId = 1, token, tokenExpires = expires };
        //    }
        //    return new { authenticated = false };
        //}

        //[HttpGet]
        //[Route("IsAuthenticated")]
        //public async Task<dynamic> IsAuthenticated()
        //{
        //    var authenticated = false;
        //    string user = null;
        //    var entityId = -1;
        //    string token = null;
        //    var tokenExpires = default(DateTime?);

        //    if (CurrentUser != null)
        //    {
        //        authenticated = CurrentUser.Identity.IsAuthenticated;
        //        if (authenticated)
        //        {
        //            user = CurrentUser.Identity.Name;
        //            foreach (var c in CurrentUser.Claims)
        //            {
        //                if (c.Type == "EntityID") entityId = Convert.ToInt32(c.Value);
        //            }
        //            tokenExpires = DateTime.UtcNow.AddMinutes(2);
        //            token = await GetTokenAsync(CurrentUser.Email, tokenExpires);
        //        }
        //    }
        //    return new {authenticated, user, entityId, token, tokenExpires };
        //}
        #endregion
    }
}
