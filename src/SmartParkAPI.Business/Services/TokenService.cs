using System;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json;
using SmartParkAPI.Business.Providers;
using SmartParkAPI.Business.Services.Base;
using SmartParkAPI.Contracts.Common;
using SmartParkAPI.Contracts.DTO.Token;
using SmartParkAPI.Contracts.Services;
using SmartParkAPI.DataAccess.Common;
using SmartParkAPI.DataAccess.Interfaces;
using SmartParkAPI.Model.Concrete;
using SmartParkAPI.Shared.Enums;
using SmartParkAPI.Shared.Helpers;

namespace SmartParkAPI.Business.Services
{
    public class TokenService : EntityService<TokenBaseDto, Token, long>, ITokenService
    {
        private readonly ITokenRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TokenService(IUnitOfWork unitOfWork, ITokenRepository repository, IMapper mapper)
            : base(repository, unitOfWork, mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public ServiceResult<TokenBaseDto> Create(TokenType tokenType)
        {
            return base.Create(GetTokenToAdd(tokenType));
        }

        public async Task<ServiceResult<TokenBaseDto>> CreateAsync(TokenType tokenType)
        {
            return await base.CreateAsync(GetTokenToAdd(tokenType));
        }

        public ServiceResult<SplittedTokenData> GetDecryptedData(string encryptedData)
        {
            var decryptedData = EncryptHelper.Decrypt(encryptedData);
            var tokenDto = JsonConvert.DeserializeObject<TokenBaseDto>(decryptedData);
            return ServiceResult<SplittedTokenData>.Success(_mapper.Map<SplittedTokenData>(tokenDto));
        }

        public async Task<ServiceResult<TokenBaseDto>> GetTokenBySecureTokenAndTypeAsync(Guid secureToken, TokenType type)
        {
            var token = _mapper.Map<TokenBaseDto>(await _repository.FirstOrDefaultAsync(x => x.TokenType == type && x.SecureToken == secureToken));
            if (token == null)
            {
                return ServiceResult<TokenBaseDto>.Failure("Not found");
            }
            return token.NotExpired()
                ? ServiceResult<TokenBaseDto>.Success(token)
                : ServiceResult<TokenBaseDto>.Failure("Expired");
        }

        public async Task<ServiceResult> DeleteTokenBySecureTokenAndTypeAsync(Guid secureToken, TokenType type)
        {
            var token = await _repository.FirstOrDefaultAsync(x => x.TokenType == type && x.SecureToken == secureToken);
            _repository.Delete(token);
            await _unitOfWork.CommitAsync();
            return ServiceResult.Success();
        }

        private static TokenBaseDto GetTokenToAdd(TokenType tokenType)
        {
            return new TokenBaseDto
            {
                SecureToken = Guid.NewGuid(),
                TokenType = tokenType,
                ValidTo = TokenValidityTimeProvider.GetValidToDate(tokenType)
            };
        }
    }
}
