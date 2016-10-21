using System.Net.Mail.Abstractions;
using AutoMapper;
using SmartParkAPI.Contracts.DTO;
using SmartParkAPI.Model.Concrete;
using SmartParkAPI.Shared.Helpers;

namespace SmartParkAPI.Resolver.Mappings
{
    public class MessageBackendMappings : Profile
    {
        protected override void Configure()
        {
            CreateMap<Message, MessageDto>().IgnoreNotExistingProperties();
            CreateMap<MessageDto, Message>().IgnoreNotExistingProperties();
            CreateMap<SmtpSettings, SmtpClient>()
                .AfterMap((src, dest) =>
                {
                    dest.Credentials = src.Credentials;
                }).IgnoreNotExistingProperties();
        }
    }
}
