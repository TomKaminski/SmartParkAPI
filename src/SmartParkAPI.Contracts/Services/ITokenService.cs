using System;
using System.Threading.Tasks;
using SmartParkAPI.Contracts.Common;
using SmartParkAPI.Contracts.DTO.Token;
using SmartParkAPI.Contracts.Services.Base;
using SmartParkAPI.Shared.Enums;

namespace SmartParkAPI.Contracts.Services
{
    public interface ITokenService:IEntityService<TokenBaseDto,long>, IDependencyService
    {
        ServiceResult<TokenBaseDto> Create(TokenType tokenType);
        Task<ServiceResult<TokenBaseDto>> CreateAsync(TokenType tokenType);
        ServiceResult<SplittedTokenData> GetDecryptedData(string encryptedData);

        Task<ServiceResult<TokenBaseDto>> GetTokenBySecureTokenAndTypeAsync(Guid secureToken, TokenType type);
        Task<ServiceResult> DeleteTokenBySecureTokenAndTypeAsync(Guid secureToken, TokenType type);
    }
}
