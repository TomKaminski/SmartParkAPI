using System;
using AutoMapper;
using SmartParkAPI.Contracts.DTO;
using SmartParkAPI.Contracts.Services.Base;
using SmartParkAPI.Controllers.Admin.Base;
using SmartParkAPI.Models.Admin.Message;

namespace SmartParkAPI.Controllers.Admin
{
    public class AdminMessageController : AdminServiceBaseController<AdminMessageListItemViewModel, MessageDto, Guid>
    {
        public AdminMessageController(IEntityService<MessageDto, Guid> entityService, IMapper mapper) : base(entityService, mapper)
        {
        }
    }
}
