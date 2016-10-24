using System.Net.Mail.Abstractions;
using AutoMapper;
using SmartParkAPI.Contracts.DTO;
using SmartParkAPI.Model.Concrete;
using SmartParkAPI.Shared.Helpers;

namespace SmartParkAPI.Resolver.Mappings
{
    public class MessageBackendMappings : Profile
    {
        public MessageBackendMappings()
        {
            CreateMap<Message, MessageDto>();
            CreateMap<MessageDto, Message>();
            CreateMap<SmtpSettings, SmtpClient>()
                .AfterMap((src, dest) =>
                {
                    dest.Credentials = src.Credentials;
                });
        }
    }
}
