using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartParkAPI.Business.Services.Base;
using SmartParkAPI.Contracts.Common;
using SmartParkAPI.Contracts.DTO.GateUsage;
using SmartParkAPI.Contracts.DTO.User;
using SmartParkAPI.Contracts.DTO.UserPreferences;
using SmartParkAPI.Contracts.Services;
using SmartParkAPI.DataAccess.Common;
using SmartParkAPI.DataAccess.Interfaces;
using SmartParkAPI.Model.Concrete;
using SmartParkAPI.Shared.Enums;
using SmartParkAPI.Shared.Helpers;
using SmartParkAPI.Shared.PasswordHash;

namespace SmartParkAPI.Business.Services
{
    public class UserService : EntityService<UserBaseDto, User, int>, IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IGateUsageRepository _gateUsageRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly ITokenRepository _tokenRepository;
        private readonly IUserPreferencesRepository _userPreferencesRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository repository, IUnitOfWork unitOfWork, IGateUsageRepository gateUsageRepository, IPasswordHasher passwordHasher, ITokenService tokenService, ITokenRepository tokenRepository, IUserPreferencesRepository userPreferencesRepository, IMapper mapper)
            : base(repository, unitOfWork, mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _gateUsageRepository = gateUsageRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _tokenRepository = tokenRepository;
            _userPreferencesRepository = userPreferencesRepository;
            _mapper = mapper;
        }

        public override async Task<ServiceResult> DeleteAsync(int id)
        {
            var obj = await _repository.FindAsync(id);
            if (obj.IsAdmin)
            {
                return ServiceResult.Failure("Nie można usunąć konta administratora!");
            }
            obj.IsDeleted = true;
            _repository.Edit(obj);
            await _unitOfWork.CommitAsync();
            return ServiceResult.Success();
        }

        public async Task<ServiceResult> RecoverUserAsync(int id)
        {
            var obj = await _repository.FindAsync(id);
            obj.IsDeleted = false;
            _repository.Edit(obj);
            await _unitOfWork.CommitAsync();
            return ServiceResult.Success();
        }

        public async Task<ServiceResult> AdminEditAsync(UserBaseDto user, string oldEmail)
        {
            if (oldEmail == user.Email || await _repository.FirstOrDefaultAsync(x => x.Email == user.Email) == null)
            {
                var entity = await _repository.FirstOrDefaultAsync(x => x.Email == oldEmail);
                _mapper.Map(user, entity);

                _repository.Edit(entity);
                await _unitOfWork.CommitAsync();
                return ServiceResult.Success();
            }
            return ServiceResult<UserBaseDto, UserPreferencesDto>.Failure("Podany adres email jest już zajęty");
        }

        public async Task<ServiceResult<bool>> CheckHashAsync(string email, string hash)
        {
            var stud = await _repository.FirstOrDefaultAsync(x => x.Email == email);
            if (stud != null && stud.PasswordHash == hash && !stud.LockedOut && !stud.IsDeleted)
            {
                return ServiceResult<bool>.Success(true);
            }
            return ServiceResult<bool>.Failure("Niepoprawny login lub hasło");
        }

        public new ServiceResult<UserBaseDto> Create(UserBaseDto entity)
        {
            var possibleUserExistResult = _repository.FirstOrDefault(x => x.Email == entity.Email);
            if (possibleUserExistResult == null || possibleUserExistResult.IsDeleted)
            {
                var code = GetUniqueKey();
                var saltHash = _passwordHasher.CreateHash(code);

                if (possibleUserExistResult == null)
                {
                    return CreateNewUser(entity, saltHash);
                }
                return UnDeleteExistingUser(possibleUserExistResult, saltHash);
            }
            return ServiceResult<UserBaseDto>.Failure("Adres email jest już zajęty");
        }

        private ServiceResult<UserBaseDto> UnDeleteExistingUser(User possibleUserExistResult, EncryptedPasswordData saltHash)
        {
            possibleUserExistResult.PasswordHash = saltHash.Hash;
            possibleUserExistResult.PasswordSalt = saltHash.Salt;
            possibleUserExistResult.IsDeleted = false;
            _repository.Edit(_mapper.Map<User>(possibleUserExistResult));
            _unitOfWork.Commit();
            return ServiceResult<UserBaseDto>.Success(_mapper.Map<UserBaseDto>(possibleUserExistResult));
        }

        public ServiceResult<UserBaseDto> Create(UserBaseDto entity, string password)
        {
            var possibleUserExistResult = _repository.FirstOrDefault(x => x.Email == entity.Email);
            if (possibleUserExistResult == null || possibleUserExistResult.IsDeleted)
            {
                var saltHash = _passwordHasher.CreateHash(password);

                if (possibleUserExistResult == null)
                {
                    return CreateNewUser(entity, saltHash);
                }
                return UnDeleteExistingUser(possibleUserExistResult, saltHash);
            }
            return ServiceResult<UserBaseDto>.Failure("Adres email jest już zajęty");
        }

        public new async Task<ServiceResult<UserBaseDto>> CreateAsync(UserBaseDto entity)
        {
            var possibleUserExistResult = await _repository.FirstOrDefaultAsync(x => x.Email == entity.Email);
            if (possibleUserExistResult == null || possibleUserExistResult.IsDeleted)
            {
                var code = GetUniqueKey();
                var saltHash = _passwordHasher.CreateHash(code);

                if (possibleUserExistResult == null)
                {
                    return await CreateNewUserAsync(entity, saltHash);
                }
                return await UnDeleteExistingUserAsync(possibleUserExistResult, saltHash);
            }
            return ServiceResult<UserBaseDto>.Failure("Adres email jest już zajęty");
        }



        public async Task<ServiceResult<UserBaseDto>> CreateAsync(UserBaseDto entity, string password)
        {
            var possibleUserExistResult = await _repository.FirstOrDefaultAsync(x => x.Email == entity.Email);
            if (possibleUserExistResult == null || possibleUserExistResult.IsDeleted)
            {
                var saltHash = _passwordHasher.CreateHash(password);

                if (possibleUserExistResult == null)
                {
                    return await CreateNewUserAsync(entity, saltHash);
                }
                return await UnDeleteExistingUserAsync(possibleUserExistResult, saltHash);
            }
            return ServiceResult<UserBaseDto>.Failure("Adres email jest już zajęty");
        }

        private async Task<ServiceResult<UserBaseDto>> CreateNewUserAsync(UserBaseDto entity, EncryptedPasswordData saltHash)
        {
            entity.PasswordSalt = saltHash.Salt;
            entity.PasswordHash = saltHash.Hash;
            var user = _repository.Add(_mapper.Map<User>(entity));
            var userPreference = _userPreferencesRepository.Add(new UserPreferences { UserId = user.Id, ShrinkedSidebar = false });
            await _unitOfWork.CommitAsync();
            user.UserPreferencesId = userPreference.Id;
            await _unitOfWork.CommitAsync();
            return ServiceResult<UserBaseDto>.Success(_mapper.Map<UserBaseDto>(user));
        }

        public ServiceResult<UserBaseDto> GetByEmail(string email)
        {
            var stud = _repository.FirstOrDefault(x => x.Email == email);
            return stud == null ? ServiceResult<UserBaseDto>.Failure("Użytkownik nie istnieje")
                : ServiceResult<UserBaseDto>.Success(_mapper.Map<UserBaseDto>(stud));
        }

        public ServiceResult<UserBaseDto, UserPreferencesDto> GetByEmailWithPreferences(string email)
        {
            var stud = _repository.Include(x => x.UserPreferences).FirstOrDefault(x => x.Email == email);
            return stud == null ? ServiceResult<UserBaseDto, UserPreferencesDto>.Failure("Użytkownik nie istnieje")
                : ServiceResult<UserBaseDto, UserPreferencesDto>.Success(_mapper.Map<UserBaseDto>(stud), _mapper.Map<UserPreferencesDto>(stud.UserPreferences));
        }

        public async Task<ServiceResult<UserBaseDto, UserPreferencesDto>> ChangeEmailAsync(string email, string newEmail, string password)
        {
            if ((await _repository.FirstOrDefaultAsync(x => x.Email == newEmail)) == null)
            {
                var entity = await _repository.Include(x => x.UserPreferences).FirstOrDefaultAsync(x => x.Email == email);
                if (entity != null && _passwordHasher.ValidatePassword(password, entity.PasswordHash, entity.PasswordSalt))
                {
                    entity.Email = newEmail;
                    _repository.Edit(entity);
                    await _unitOfWork.CommitAsync();
                    return ServiceResult<UserBaseDto, UserPreferencesDto>
                        .Success(_mapper.Map<UserBaseDto>(entity), _mapper.Map<UserPreferencesDto>(entity.UserPreferences));
                }
                return ServiceResult<UserBaseDto, UserPreferencesDto>.Failure("Niepoprawny login lub hasło");
            }
            return ServiceResult<UserBaseDto, UserPreferencesDto>.Failure("Adres email jest już zajęty");
        }

        public async Task<ServiceResult<UserBaseDto, string>> GetPasswordChangeTokenAsync(string email)
        {
            var entity = await _repository.Include(x => x.PasswordChangeToken).FirstOrDefaultAsync(x => x.Email == email);
            var resetPasswordToken = await _tokenService.CreateAsync(TokenType.ResetPasswordToken);
            if (entity.PasswordChangeTokenId != null)
            {
                _tokenRepository.Delete(entity.PasswordChangeToken);
            }
            entity.PasswordChangeTokenId = resetPasswordToken.Result.Id;
            _repository.Edit(entity);
            await _unitOfWork.CommitAsync();
            return ServiceResult<UserBaseDto, string>.Success(_mapper.Map<UserBaseDto>(entity), resetPasswordToken.Result.BuildEncryptedToken());
        }

        //public async Task<ServiceResult<UserBaseDto, string>> GetSelfDeleteTokenAsync(string email)
        //{
        //    var entity = await _repository.Include(x => x.SelfDeleteToken).FirstOrDefaultAsync(x => x.Email == email);
        //    var selfDeleteToken = await _tokenService.CreateAsync(TokenType.SelfDeleteToken);
        //    if (entity.SelfDeleteTokenId != null)
        //    {
        //        _tokenRepository.Delete(entity.SelfDeleteToken);
        //    }
        //    entity.SelfDeleteTokenId = selfDeleteToken.Result.Id;
        //    _repository.Edit(entity);
        //    await _unitOfWork.CommitAsync();
        //    return ServiceResult<UserBaseDto, string>.Success(_mapper.Map<UserBaseDto>(entity), selfDeleteToken.Result.BuildEncryptedToken());
        //}

        public async Task<ServiceResult<UserBaseDto>> ResetPasswordAsync(string token, string newPassword)
        {
            var decryptedTokenData = _tokenService.GetDecryptedData(token);
            var entity = await _repository.FirstOrDefaultAsync(x => x.PasswordChangeTokenId == decryptedTokenData.Result.Id);
            if (entity != null && decryptedTokenData.Result.TokenType == TokenType.ResetPasswordToken)
            {
                var newHashedPassword = _passwordHasher.CreateHash(newPassword);
                entity.PasswordSalt = newHashedPassword.Salt;
                entity.PasswordHash = newHashedPassword.Hash;
                _repository.Edit(entity);
                _tokenService.Delete(decryptedTokenData.Result.Id);
                await _unitOfWork.CommitAsync();
                return ServiceResult<UserBaseDto>.Success(_mapper.Map<UserBaseDto>(entity));
            }
            return ServiceResult<UserBaseDto>.Failure("Nieważny token zmiany hasła.");
        }

        public async Task<ServiceResult<UserBaseDto>> ChangePasswordAsync(string email, string password, string newPassword)
        {
            var entity = await _repository.FirstOrDefaultAsync(x => x.Email == email);
            if (entity != null)
            {
                if (_passwordHasher.ValidatePassword(password, entity.PasswordHash, entity.PasswordSalt))
                {
                    var newHashedPassword = _passwordHasher.CreateHash(newPassword);
                    entity.PasswordSalt = newHashedPassword.Salt;
                    entity.PasswordHash = newHashedPassword.Hash;
                    _repository.Edit(entity);
                    await _unitOfWork.CommitAsync();
                    return ServiceResult<UserBaseDto>.Success(_mapper.Map<UserBaseDto>(entity));
                }
                return ServiceResult<UserBaseDto>.Failure("Podane obecne hasło jest niepoprawne.");

            }
            return ServiceResult<UserBaseDto>.Failure("Wystąpił błąd podczas zmiany hasła, spróbuj jeszcze raz.");
        }

        public async Task<ServiceResult<int>> GetChargesAsync(string email, string hash)
        {
            var stud = await _repository.FirstOrDefaultAsync(x => x.Email == email);

            return stud != null && stud.PasswordHash == hash
                ? ServiceResult<int>.Success(stud.Charges)
                : ServiceResult<int>.Failure("Użytkownik nie został znaleziony");
        }

        public async Task<ServiceResult<int>> AddChargesAsync(string email, int charges)
        {
            var entity = await _repository.FirstOrDefaultAsync(x => x.Email == email);
            entity.Charges += charges;
            _repository.Edit(entity);
            await _unitOfWork.CommitAsync();
            return ServiceResult<int>.Success(entity.Charges);
        }

        public async Task<ServiceResult<UserBaseDto, UserPreferencesDto>> LoginAsync(string email, string password)
        {
            var stud = await _repository.Include(x=>x.UserPreferences).FirstOrDefaultAsync(x => x.Email == email);
            if (stud != null && _passwordHasher.ValidatePassword(password, stud.PasswordHash, stud.PasswordSalt) && !stud.LockedOut && !stud.IsDeleted)
            {
                return ServiceResult<UserBaseDto, UserPreferencesDto>.Success(_mapper.Map<UserBaseDto>(stud), _mapper.Map<UserPreferencesDto>(stud.UserPreferences));
            }
            return ServiceResult<UserBaseDto, UserPreferencesDto>.Failure("Niepoprawny login lub hasło");
        }

        public async Task<ServiceResult<UserBaseDto, GateUsageBaseDto>> GetUserDataWithLastGateUsage(int userId)
        {
            var stud = await _repository.Include(x => x.GateUsages).FirstOrDefaultAsync(x => x.Id == userId);
            if (stud == null)
            {
                return ServiceResult<UserBaseDto, GateUsageBaseDto>.Failure("Użytkownik nie istnieje");
            }
            if (stud.GateUsages == null || stud.GateUsages.Count == 0)
            {
                return ServiceResult<UserBaseDto, GateUsageBaseDto>.Success(_mapper.Map<UserBaseDto>(stud), null);
            }
            return ServiceResult<UserBaseDto, GateUsageBaseDto>.Success(
                _mapper.Map<UserBaseDto>(stud),
                _mapper.Map<GateUsageBaseDto>(stud.GateUsages.OrderByDescending(x => x.DateOfUse).First()));
        }

        public async Task<ServiceResult<UserBaseDto>> CheckLoginAsync(string email, string hash)
        {
            var stud = await _repository.FirstOrDefaultAsync(x => x.Email == email);
            if (stud != null && stud.PasswordHash == hash && !stud.LockedOut && !stud.IsDeleted)
            {
                return ServiceResult<UserBaseDto>.Success(_mapper.Map<UserBaseDto>(stud));
            }
            return ServiceResult<UserBaseDto>.Failure("Niepoprawny login lub hasło");
        }

        //public async Task<ServiceResult<bool>> SelfDeleteAsync(string email, string token)
        //{
        //    var decryptedTokenData = _tokenService.GetDecryptedData(token);
        //    var entity = await _repository.FirstOrDefaultAsync(x => x.SelfDeleteTokenId == decryptedTokenData.Result.Id);
        //    if (entity != null && decryptedTokenData.Result.TokenType == TokenType.SelfDeleteToken)
        //    {
        //        entity.IsDeleted = true;
        //        _repository.Edit(entity);
        //        _tokenService.Delete(decryptedTokenData.Result.Id);
        //        await _unitOfWork.CommitAsync();
        //        return ServiceResult<bool>.Success(true);
        //    }
        //    return ServiceResult<bool>.Failure("Nieważny token usunięcia konta.");
        //}

        public async Task<ServiceResult<int?>> OpenGateAsync(string email, string hash)
        {
            var user = await _repository.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null || user.PasswordHash != hash)
            {
                return ServiceResult<int?>.Failure("Nie znaleziono użytkownika powiązanego z podanym loginem i hasłem");
            }
            if (user.Charges == 0)
            {
                return ServiceResult<int?>.Failure("Brak wyjazdów, doładuj konto");
            }

            var tryOpenGateResult = await TryOpenGate();

            if (tryOpenGateResult.IsValid)
            {
                --user.Charges;
                _repository.Edit(user);
                _gateUsageRepository.Add(new GateUsage
                {
                    DateOfUse = DateTime.Now,
                    UserId = user.Id
                });
                await _unitOfWork.CommitAsync();
                return ServiceResult<int?>.Success(user.Charges);
            }
            return
                ServiceResult<int?>.Failure(
                    "Usługa otwierania bramy jest obecnie wyłączona, przepraszamy za problem.");
        }

        public async Task<ServiceResult<UserBaseDto>> GetByEmailAsync(string email)
        {
            var stud = await _repository.FirstOrDefaultAsync(x => x.Email == email);
            return stud == null
                ? ServiceResult<UserBaseDto>.Failure("Użytkownik o podanym emailu nie istnieje!")
                : ServiceResult<UserBaseDto>.Success(_mapper.Map<UserBaseDto>(stud));
        }

        public async Task<ServiceResult<UserBaseDto, UserPreferencesDto>> GetByEmailWithPreferencesAsync(string email)
        {
            var stud = await _repository.Include(x => x.UserPreferences).AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);
            return stud == null ? ServiceResult<UserBaseDto, UserPreferencesDto>.Failure("Użytkownik nie istnieje")
                : ServiceResult<UserBaseDto, UserPreferencesDto>.Success(_mapper.Map<UserBaseDto>(stud), _mapper.Map<UserPreferencesDto>(stud.UserPreferences));
        }

        public async Task<ServiceResult<bool>> AccountExistsAsync(string email)
        {
            return ServiceResult<bool>.Success(await _repository.FirstOrDefaultAsync(x => x.Email == email) != null);
        }

        public async Task<ServiceResult<bool>> IsAdmin(string email)
        {
            return ServiceResult<bool>.Success((await _repository.FirstOrDefaultAsync(x => x.Email == email)).IsAdmin);
        }

        public async Task<ServiceResult<UserBaseDto, UserPreferencesDto>> EditStudentInitialsAsync(UserBaseDto entity)
        {
            var student = await _repository.Include(x => x.UserPreferences).FirstOrDefaultAsync(x => x.Email == entity.Email);
            student.Name = entity.Name;
            student.LastName = entity.LastName;
            _repository.Edit(student);
            await _unitOfWork.CommitAsync();
            return ServiceResult<UserBaseDto, UserPreferencesDto>.Success(_mapper.Map<UserBaseDto>(student), _mapper.Map<UserPreferencesDto>(student.UserPreferences));
        }

        public async Task<ServiceResult<int>> TransferCharges(string senderEmail, string recieverEmail, int numberOfCharges, string password)
        {
            var sender = await _repository.FirstOrDefaultAsync(x => x.Email == senderEmail);
            var reciever = await _repository.FirstOrDefaultAsync(x => x.Email == recieverEmail);
            if (sender != null && reciever != null && sender.Charges >= numberOfCharges && !reciever.IsDeleted)
            {
                if (_passwordHasher.ValidatePassword(password, sender.PasswordHash, sender.PasswordSalt))
                {
                    sender.Charges -= numberOfCharges;
                    reciever.Charges += numberOfCharges;
                    _repository.Edit(sender);
                    _repository.Edit(reciever);
                    await _unitOfWork.CommitAsync();
                    return ServiceResult<int>.Success(sender.Charges);
                }
                return ServiceResult<int>.Failure($"Nie można przekazać {numberOfCharges} wyjazdów na konto {recieverEmail} - złe hasło.");
            }
            return ServiceResult<int>.Failure($"Nie można przekazać {numberOfCharges} wyjazdów na konto {recieverEmail}");
        }

        public ServiceResult<IEnumerable<UserAdminDto>> GetAllAdmin()
        {
            var query = _repository.Include(x => x.Orders).Include(x => x.GateUsages).Include(x => x.UserPreferences).OrderBy(x => x.LastName);
            var result = query.Select(_mapper.Map<UserAdminDto>).ToList();

            return ServiceResult<IEnumerable<UserAdminDto>>.Success(result);
        }

        public ServiceResult<IEnumerable<UserAdminDto>> GetAllAdmin(Expression<Func<UserBaseDto, bool>> predicate)
        {
            var query = _repository.Include(x => x.Orders).Include(x => x.GateUsages).Include(x => x.UserPreferences).OrderBy(x => x.LastName).Where(MapExpressionToEntity(predicate));
            var result = query.Select(_mapper.Map<UserAdminDto>).ToList();

            return ServiceResult<IEnumerable<UserAdminDto>>.Success(result);
        }

        public async Task<ServiceResult<UserAdminDto>> GetAdminAsync(int id)
        {
            return ServiceResult<UserAdminDto>.Success(_mapper.Map<UserAdminDto>(await _repository.FirstAsync(x => x.Id == id)));
        }

        public async Task<ServiceResult<UserAdminDto>> GetAdminAsync(Expression<Func<UserBaseDto, bool>> predicate)
        {
            return ServiceResult<UserAdminDto>.Success(_mapper.Map<UserAdminDto>(await _repository.FirstAsync(MapExpressionToEntity(predicate))));
        }

        public async Task<ServiceResult<int>> GetAdminAccountIdAsync()
        {
            var adminAccount = await _repository.SingleOrDefaultAsync(x => x.IsAdmin);

            return adminAccount == null
                ? ServiceResult<int>.Failure("Could not find admin account")
                : ServiceResult<int>.Success(adminAccount.Id);
        }

        #region Helpers

        private static string GetUniqueKey()
        {
            const int size = 8;
            var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            var data = new byte[1];
            var crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            var result = new StringBuilder(size);
            foreach (var b in data)
            {
                result.Append(chars[b % (chars.Length - 1)]);
            }
            return result.ToString();
        }
        #endregion

        private async Task<ServiceResult<UserBaseDto>> UnDeleteExistingUserAsync(User possibleUserExistResult, EncryptedPasswordData saltHash)
        {
            possibleUserExistResult.PasswordHash = saltHash.Hash;
            possibleUserExistResult.PasswordSalt = saltHash.Salt;
            possibleUserExistResult.IsDeleted = false;
            _repository.Edit(_mapper.Map<User>(possibleUserExistResult));
            await _unitOfWork.CommitAsync();
            return ServiceResult<UserBaseDto>.Success(_mapper.Map<UserBaseDto>(possibleUserExistResult));
        }

        private ServiceResult<UserBaseDto> CreateNewUser(UserBaseDto entity, EncryptedPasswordData saltHash)
        {
            entity.PasswordSalt = saltHash.Salt;
            entity.PasswordHash = saltHash.Hash;
            var user = _repository.Add(_mapper.Map<User>(entity));
            var userPreference = _userPreferencesRepository.Add(new UserPreferences { UserId = user.Id, ShrinkedSidebar = false });
            _unitOfWork.Commit();
            user.UserPreferencesId = userPreference.Id;
            _unitOfWork.Commit();
            return ServiceResult<UserBaseDto>.Success(_mapper.Map<UserBaseDto>(user));
        }

        private async Task<ServiceResult<bool>> TryOpenGate()
        {
            using (var client = new HttpClient(new HttpClientHandler
            {
                AllowAutoRedirect = false
            }))
            {
                client.BaseAddress = new Uri("http://www.reset.ath.bielsko.pl");


                var response = await client.GetAsync("");

                if (response.IsSuccessStatusCode)
                {
                    var stringResult = await response.Content.ReadAsStringAsync();
                    return ServiceResult<bool>.Success(true);
                }
                return ServiceResult<bool>.Failure(response.ReasonPhrase);
            }
        }
    }
}
