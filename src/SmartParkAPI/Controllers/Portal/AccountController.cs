using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartParkAPI.Contracts.DTO.User;
using SmartParkAPI.Contracts.Services;
using SmartParkAPI.Models.Base;
using SmartParkAPI.Models.Portal.Account;
using SmartParkAPI.Shared.Enums;

namespace SmartParkAPI.Controllers.Portal
{
    [Area("Portal")]
    [Route("[area]/[controller]")]
    [Authorize]
    public class AccountController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly IMessageService _messageService;
        private readonly IMapper _mapper;

        public AccountController(IUserService userService, IMessageService messageService, IMapper mapper)
        {
            _userService = userService;
            _messageService = messageService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("~/[area]/Rejestracja")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userCreateResult = await _userService.CreateAsync(_mapper.Map<UserBaseDto>(model), model.Password);
                if (userCreateResult.IsValid)
                {
                    await _messageService.SendMessageAsync(EmailType.Register, userCreateResult.Result, GetAppBaseUrl());
                    return Ok(SmartJsonResult<RegisterViewModel>.Success(model, "Twoje konto zostało utworzone pomyślnie, czas się zalogować! :)"));
                }
                return BadRequest(SmartJsonResult.Failure(userCreateResult.ValidationErrors));
            }
            return BadRequest(SmartJsonResult.Failure(GetModelStateErrors(ModelState)));
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("ZapomnianeHaslo")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var changePasswordTokenResult = await _userService.GetPasswordChangeTokenAsync(model.Email);
                if (changePasswordTokenResult.IsValid)
                {
                    var changePasswordUrl = $"{Url.Action("RedirectFromToken", "Token", null, "http")}?id={changePasswordTokenResult.SecondResult}";
                    await _messageService.SendMessageAsync(EmailType.ResetPassword, changePasswordTokenResult.Result, GetAppBaseUrl(),
                        new Dictionary<string, string> { { "ChangePasswordLink", changePasswordUrl } });
                    return Ok(SmartJsonResult<ForgotPasswordViewModel>.Success(model, "Na podany adres email zostały wysłane dalsze instrukcje."));
                }
                return BadRequest(changePasswordTokenResult.ValidationErrors);
            }
            return BadRequest(GetModelStateErrors(ModelState));
        }
    }
}
