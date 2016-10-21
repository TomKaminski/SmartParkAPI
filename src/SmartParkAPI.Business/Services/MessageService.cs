using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net.Mail.Abstractions;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json;
using SmartParkAPI.Business.Providers.Email;
using SmartParkAPI.Business.Services.Base;
using SmartParkAPI.Contracts.Common;
using SmartParkAPI.Contracts.DTO;
using SmartParkAPI.Contracts.DTO.User;
using SmartParkAPI.Contracts.Services;
using SmartParkAPI.DataAccess.Common;
using SmartParkAPI.DataAccess.Interfaces;
using SmartParkAPI.Model.Concrete;
using SmartParkAPI.Shared.Enums;

namespace SmartParkAPI.Business.Services
{
    public class MessageService : EntityService<MessageDto, Message, Guid>, IMessageService
    {
        private readonly ISmtpClient _smtpClient;
        private readonly IEmailContentProvider _emailContentProvider;
        private readonly IMessageRepository _messageRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;


        private readonly SmtpSettings _smtpSettings;

        private const string TokenRedirectFormat = "/Redirect?id={0}";

        public MessageService(IUnitOfWork unitOfWork, ISmtpClient smtpClient, IAppSettingsProvider appSettingsProvider, 
            IEmailContentProvider emailContentProvider, IMessageRepository messageRepository, ITokenService tokenService, IMapper mapper) 
            : base(messageRepository, unitOfWork, mapper)
        {
            _smtpSettings = appSettingsProvider.GetSmtpSettings();
            _smtpClient = smtpClient;
            _mapper = mapper;
            _smtpClient = _mapper.Map<System.Net.Mail.Abstractions.SmtpClient>(_smtpSettings);

            _emailContentProvider = emailContentProvider;
            _messageRepository = messageRepository;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;

            appSettingsProvider.GetAppSettings(AppSettingsType.DefaultSettings, AppSettingsType.Resources);
        }

        public async Task<ServiceResult> SendMessageAsync(EmailType type, UserBaseDto userData, string appBasePath, 
            Dictionary<string,string> additionalParameters = null)
        {
            var tokenCreateResult = await _tokenService.CreateAsync(TokenType.ViewInBrowserToken);

            var emailParameters = EmailParametersProvider.GetParameters(userData,additionalParameters);
            emailParameters.Add("ViewInBrowserLink", string.Format(appBasePath + TokenRedirectFormat, tokenCreateResult.Result.BuildEncryptedToken()));

            var messageDto = new MessageDto {Type = type};
            var emailBody = _emailContentProvider.GetEmailBody(messageDto.Type, emailParameters);
            messageDto.To = userData.Email;
            messageDto.DisplayFrom = "Parking ATH";
            messageDto.Title = _emailContentProvider.GetEmailTitle(messageDto.Type);
            messageDto.MessageParameters = JsonConvert.SerializeObject(emailParameters);
            messageDto.UserId = userData.Id;
            messageDto.From = _smtpSettings.From;
            messageDto.ViewInBrowserTokenId = tokenCreateResult.Result.Id;

            _messageRepository.Add(_mapper.Map<Message>(messageDto));
            await _unitOfWork.CommitAsync();

            var mailMessage = new MailMessage(messageDto.From, userData.Email, messageDto.Title, emailBody)
            {
                IsBodyHtml = true
            };
            await _smtpClient.SendMailAsync(mailMessage);
            return ServiceResult.Success();
        }

        public ServiceResult<string> GetMessageBody(MessageDto message)
        {
            return ServiceResult<string>.Success(_emailContentProvider.GetEmailBody(message.Type, JsonConvert.DeserializeObject<Dictionary<string, string>>(message.MessageParameters)));
        }

        public async Task<ServiceResult<MessageDto>> GetMessageByTokenId(long id)
        {
            return ServiceResult<MessageDto>.Success(_mapper.Map<MessageDto>(await _messageRepository.GetMessageByTokenId(id)));
        }

        public Task<ServiceResult<bool>> ValidateMessageRecipents(int userId, Guid previousMessageId)
        {
            throw new NotImplementedException();
        }
    }
}
