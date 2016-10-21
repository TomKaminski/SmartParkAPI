using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartParkAPI.Contracts.Common;
using SmartParkAPI.Contracts.DTO;
using SmartParkAPI.Contracts.DTO.User;
using SmartParkAPI.Contracts.Services.Base;
using SmartParkAPI.Shared.Enums;

namespace SmartParkAPI.Contracts.Services
{
    public interface IMessageService: IEntityService<MessageDto, Guid>,IDependencyService
    {
        Task<ServiceResult> SendMessageAsync(EmailType type, UserBaseDto userData, string appBasePath, Dictionary<string, string> additionalParameters = null);
        ServiceResult<string> GetMessageBody(MessageDto message);
        Task<ServiceResult<MessageDto>> GetMessageByTokenId(long id);
    }
}
