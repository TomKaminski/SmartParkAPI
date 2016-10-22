using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Policy;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using SmartParkAPI.Contracts.DTO.User;
using SmartParkAPI.Contracts.DTO.UserPreferences;
using SmartParkAPI.Contracts.Services;
using SmartParkAPI.Model.Concrete;
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

        [Route("Wyloguj")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            //IdentitySignout();
            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        [Route("~/[area]/Logowanie")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userLoginResult = await _userService.LoginAsync(model.Email, model.Password);
                if (userLoginResult.IsValid)
                {
                    //await IdentitySignin(userLoginResult.Result, userLoginResult.SecondResult, model.RememberMe);
                }
                model.AppendErrors(userLoginResult.ValidationErrors);
            }
            model.AppendErrors(GetModelStateErrors(ModelState));
            if (model.ReturnUrl == null)
            {
                model.ReturnUrl = Url.Action("Index", "Home", new { area = "Portal" });
            }
            return Json(model);
        }


        [HttpPost]
        [Route("~/[area]/Rejestracja")]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userCreateResult = await _userService.CreateAsync(_mapper.Map<UserBaseDto>(model), model.Password);
                if (userCreateResult.IsValid)
                {
                    await _messageService.SendMessageAsync(EmailType.Register, userCreateResult.Result, GetAppBaseUrl());
                    model.AppendNotifications("Twoje konto zostało utworzone pomyślnie, czas się zalogować! :)");
                }
                model.AppendErrors(userCreateResult.ValidationErrors);
            }
            model.AppendErrors(GetModelStateErrors(ModelState));
            return Json(model);
        }

        [AllowAnonymous]
        [Route("ZapomnianeHaslo")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
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
                    model.AppendNotifications("Na podany adres email zostały wysłane dalsze instrukcje.");
                }
                model.AppendErrors(changePasswordTokenResult.ValidationErrors);
            }
            model.AppendErrors(GetModelStateErrors(ModelState));
            return Json(model);
        }

     
    }
}
