using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartParkAPI.Contracts.DTO.PortalMessage;
using SmartParkAPI.Contracts.Services;
using SmartParkAPI.Models.Base;
using SmartParkAPI.Models.Portal.Message;
using SmartParkAPI.Models.Portal.PortalMessage;
using SmartParkAPI.Shared.Enums;

namespace SmartParkAPI.Controllers.Portal
{
    [Area("Portal")]
    [Route("[area]/[controller]")]
    [Authorize]
    public class MessageController : BaseApiController
    {
        private readonly IMessageService _messageService;
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;
        private readonly IPortalMessageService _portalMessageService;
        private readonly IMapper _mapper;

        public MessageController(IMessageService messageService, ITokenService tokenService, IUserService userService, IPortalMessageService portalMessageService, IMapper mapper)
        {
            _messageService = messageService;
            _tokenService = tokenService;
            _userService = userService;
            _portalMessageService = portalMessageService;
            _mapper = mapper;
        }


        [AllowAnonymous]
        [Route("Podglad")]
        public async Task<IActionResult> Display(string id)
        {
            var decodedToken = _tokenService.GetDecryptedData(id);
            var tokenData = await _tokenService.GetTokenBySecureTokenAndTypeAsync(decodedToken.Result.SecureToken, decodedToken.Result.TokenType);
            var message = await _messageService.GetMessageByTokenId(tokenData.Result.Id);
            var emailBody = _messageService.GetMessageBody(message.Result).Result;

            return View(new DisplayMessageViewModel
            {
                EmailHtml = emailBody,
                Title = message.Result.Title
            });
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> SetDisplayed([FromBody] SetDisplayedMessageViewModel model)
        {
            if (ModelState.IsValid)
            {
                var message = await _portalMessageService.GetAsync(model.MessageId);
                if (message != null)
                {
                    message.Result.IsDisplayed = true;
                    await _portalMessageService.EditAsync(message.Result);
                    return Json(SmartJsonResult.Success());
                }
            }
            return Json(SmartJsonResult.Failure());
        }

        //[HttpPost]
        //[Route("[action]")]
        //public async Task<IActionResult> SendQuickMessage([FromBody] QuickMessageViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        //Cant be null coz user is logged in
        //        model.UserId = CurrentUser.UserId.Value;

        //        var adminAccountResult = await _userService.GetAdminAccountIdAsync();
        //        if (!adminAccountResult.IsValid)
        //        {
        //            model.AppendErrors(adminAccountResult.ValidationErrors);
        //        }
        //        else
        //        {
        //            model.ReceiverUserId = adminAccountResult.Result;
        //            var serviceRequest = _mapper.Map<PortalMessageDto>(model);
        //            serviceRequest.Starter = true;
        //            var createResult = await _portalMessageService.CreateAsync(serviceRequest);
        //            if (createResult.IsValid)
        //            {
        //                model.AppendNotifications("Wiadomość wysłana, dziękujemy za wsparcie :)");
        //            }
        //            else
        //            {
        //                model.AppendErrors(createResult.ValidationErrors);
        //            }
        //        }
        //    }
        //    model.AppendErrors(GetModelStateErrors(ModelState));
        //    return Json(model);
        //}


        //[HttpPost]
        //[Route("GetUserMessagesClusters")]
        //public async Task<IActionResult> GetUserMessagesClusters([FromBody] int skipNumber = 0)
        //{
        //    var messagesGetResult = await _portalMessageService.GetPortalMessageClusterForCurrentUserAsync(CurrentUser.UserId.Value);
        //    if (messagesGetResult.IsValid)
        //    {
        //        var jsonResult = _mapper.Map<PortalMessageClustersViewModel>(messagesGetResult.Result);
        //        return Json(SmartJsonResult<PortalMessageClustersViewModel>.Success(jsonResult));
        //    }
        //    return Json(SmartJsonResult.Failure(messagesGetResult.ValidationErrors));
        //}

        //[HttpPost]
        //[Route("GetUnreadClustersCount")]
        //public async Task<IActionResult> GetUnreadClustersCount()
        //{
        //    var messagesGetResult = await _portalMessageService.GetPortalMessageClusterForCurrentUserAsync(CurrentUser.UserId.Value);
        //    if (messagesGetResult.IsValid)
        //    {
        //        var jsonResult = _mapper.Map<PortalMessageClustersViewModel>(messagesGetResult.Result);
        //        return Json(SmartJsonResult<PortalMessageClustersViewModel>.Success(jsonResult));
        //    }
        //    return Json(SmartJsonResult.Failure(messagesGetResult.ValidationErrors));
        //}


        //[HttpPost]
        //[Route("[action]")]
        //public async Task<IActionResult> ReplyPortalMessage([FromBody] ReplyMessageViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var validateMessageResult = await _portalMessageService.ValidateMessageRecipents(CurrentUser.UserId.Value, model.PreviousMessageId);
        //        if (!validateMessageResult.IsValid)
        //        {
        //            return Json(SmartJsonResult.Failure(validateMessageResult.ValidationErrors));
        //        }
        //        PrepareReplyMessageModel(model, validateMessageResult.Result);

        //        var serviceRequest = _mapper.Map<PortalMessageDto>(model);
        //        serviceRequest.Starter = false;

        //        var createResult = await _portalMessageService.CreateAsync(serviceRequest);
        //        if (createResult.IsValid)
        //        {
        //            return Json(SmartJsonResult<PortalMessageItemViewModel>.Success(_mapper.Map<PortalMessageItemViewModel>(createResult.Result), "Wiadomość została wysłana"));
        //        }
        //        return Json(SmartJsonResult.Failure(createResult.ValidationErrors));
        //    }
        //    return Json(SmartJsonResult.Failure(GetModelStateErrors(ModelState)));
        //}

        //[HttpPost]
        //[Route("[action]")]
        //public async Task<IActionResult> FakeDeleteCluster([FromBody]FakeDeleteClusterViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var fakeDeleteClusterResult = await _portalMessageService.FakeDeleteCluster(CurrentUser.UserId.Value, model.StarterMessageId);
        //        if (!fakeDeleteClusterResult.IsValid)
        //        {
        //            return Json(SmartJsonResult.Failure(fakeDeleteClusterResult.ValidationErrors));
        //        }
        //        return Json(SmartJsonResult<bool>.Success(true, "Konwersacja została poprawnie usunięta."));
        //    }
        //    return Json(SmartJsonResult.Failure(GetModelStateErrors(ModelState)));
        //}

        //private void PrepareReplyMessageModel(ReplyMessageViewModel model, PortalMessageDto lastMessage)
        //{
        //    model.UserId = CurrentUser.UserId.Value;
        //    model.ToAdmin = !CurrentUser.IsAdmin;
        //    model.PortalMessageType = CurrentUser.IsAdmin
        //        ? PortalMessageEnum.MessageToUserFromAdmin
        //        : PortalMessageEnum.MessageToAdminFromUser;
        //    model.ReceiverUserId = CurrentUser.UserId.Value == lastMessage.ReceiverUserId
        //        ? lastMessage.UserId
        //        : lastMessage.ReceiverUserId;
        //}

    }
}
