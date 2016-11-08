//using System.Collections.Generic;
//using System.Threading.Tasks;
//using AutoMapper;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using SmartParkAPI.Contracts.DTO.User;
//using SmartParkAPI.Contracts.Services;
//using SmartParkAPI.Models.Portal.Account;
//using SmartParkAPI.Shared.Enums;

//namespace SmartParkAPI.Controllers.Portal
//{
//    [Route("[area]/[controller]")]
//    [Authorize]
//    public class AccountController : BaseApiController
//    {
//        private readonly IUserService _userService;
//        private readonly IMessageService _messageService;
//        private readonly IMapper _mapper;

//        public AccountController(IUserService userService, IMessageService messageService, IMapper mapper)
//        {
//            _userService = userService;
//            _messageService = messageService;
//            _mapper = mapper;
//        }

//        [HttpPost]
//        [Route("~/[area]/Rejestracja")]
//        [AllowAnonymous]
//        public async Task<IActionResult> Register(RegisterViewModel model)
//        {
//            if (ModelState.IsValid)
//            {
//                var userCreateResult = await _userService.CreateAsync(_mapper.Map<UserBaseDto>(model), model.Password);
//                if (userCreateResult.IsValid)
//                {
//                    await _messageService.SendMessageAsync(EmailType.Register, userCreateResult.Result, GetAppBaseUrl());
//                    model.AppendNotifications("Twoje konto zostało utworzone pomyślnie, czas się zalogować! :)");
//                }
//                model.AppendErrors(userCreateResult.ValidationErrors);
//            }
//            model.AppendErrors(GetModelStateErrors(ModelState));
//            return Json(model);
//        }

//        [HttpPost]
//        [AllowAnonymous]
//        [ValidateAntiForgeryToken]
//        [Route("ZapomnianeHaslo")]
//        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
//        {
//            if (ModelState.IsValid)
//            {
//                var changePasswordTokenResult = await _userService.GetPasswordChangeTokenAsync(model.Email);
//                if (changePasswordTokenResult.IsValid)
//                {
//                    var changePasswordUrl = $"{Url.Action("RedirectFromToken", "Token", null, "http")}?id={changePasswordTokenResult.SecondResult}";
//                    await _messageService.SendMessageAsync(EmailType.ResetPassword, changePasswordTokenResult.Result, GetAppBaseUrl(),
//                        new Dictionary<string, string> { { "ChangePasswordLink", changePasswordUrl } });
//                    model.AppendNotifications("Na podany adres email zostały wysłane dalsze instrukcje.");
//                }
//                model.AppendErrors(changePasswordTokenResult.ValidationErrors);
//            }
//            model.AppendErrors(GetModelStateErrors(ModelState));
//            return Json(model);
//        }
//    }
//}
