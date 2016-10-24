using AutoMapper;
using SmartParkAPI.Contracts.DTO.Token;
using SmartParkAPI.Model.Concrete;
using SmartParkAPI.Shared.Helpers;

namespace SmartParkAPI.Resolver.Mappings
{
    public class TokenBackendMappings : Profile
    {
        public TokenBackendMappings()
        {
            CreateMap<Token, TokenBaseDto>();

            CreateMap<TokenBaseDto, Token>();

            CreateMap<SplittedTokenData, TokenBaseDto>();

            CreateMap<TokenBaseDto, SplittedTokenData>();
        }
    }
}
