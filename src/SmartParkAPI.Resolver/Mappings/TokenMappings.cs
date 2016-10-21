using AutoMapper;
using SmartParkAPI.Contracts.DTO.Token;
using SmartParkAPI.Model.Concrete;
using SmartParkAPI.Shared.Helpers;

namespace SmartParkAPI.Resolver.Mappings
{
    public class TokenBackendMappings : Profile
    {
        protected override void Configure()
        {
            CreateMap<Token, TokenBaseDto>().IgnoreNotExistingProperties();

            CreateMap<TokenBaseDto, Token>().IgnoreNotExistingProperties();

            CreateMap<SplittedTokenData, TokenBaseDto>().IgnoreNotExistingProperties();

            CreateMap<TokenBaseDto, SplittedTokenData>().IgnoreNotExistingProperties();
        }
    }
}
