using Autofac.Extras.Moq;
using SharpTestsEx;
using SmartParkAPI.Business.Services;
using SmartParkAPI.Business.Tests.Base;
using SmartParkAPI.DataAccess;
using SmartParkAPI.DataAccess.Common;
using SmartParkAPI.DataAccess.Repositories;
using SmartParkAPI.Shared.Helpers;
using Xunit;

namespace SmartParkAPI.Business.Tests.Services
{
    public class UserServiceTests : BusinessTestBase
    {
        private UserService _sut;
        private readonly AutoMock _mock = AutoMock.GetLoose();
        private IUnitOfWork _unitOfWork;
        private UserRepository _userRepository;
        private TokenService _tokenService;

        public UserServiceTests()
        {
            InitContext();

            var tkaminskiUser = GetUserBaseDto();
            tkaminskiUser.Email = "tkaminski93@gmail.com";
            _sut.Create(tkaminskiUser, BasicUserPassword);
            _sut.Create(GetUserBaseDto(), BasicUserPassword);
            _sut.Create(GetUserBaseDto(), BasicUserPassword);
            _sut.Create(GetUserBaseDto(), BasicUserPassword);
        }

        private void InitContext()
        {
            _mock.Mock<IDatabaseFactory>().Setup(x => x.Get()).Returns(GetContext());
            _userRepository = _mock.Create<UserRepository>();
            _unitOfWork = _mock.Create<UnitOfWork>();

            var gateRepositoryMock = _mock.Create<GateUsageRepository>();
            var passwordHasher = _mock.Create<PasswordHasher>();
            var tokenRepositoryMock = _mock.Create<TokenRepository>();
            var userPreferencesRepositoryMock = _mock.Create<UserPreferencesRepository>();
            _tokenService = new TokenService(_unitOfWork, _mock.Create<TokenRepository>(), Mapper);

            _sut = new UserService(_userRepository, _unitOfWork, gateRepositoryMock, passwordHasher, _tokenService, tokenRepositoryMock, userPreferencesRepositoryMock, Mapper);
        }

        [Fact]
        public void CreateUserWithRandomPassword_ThenResultIsValid()
        {
            //before
            var userToCreate = GetUserBaseDto();

            //act
            var userCreateResult = _sut.Create(userToCreate, BasicUserPassword);

            //then
            userCreateResult.IsValid.Should().Be.True();
            userCreateResult.Result.Should().Not.Be.Null();
        }

        [Fact]
        public async void CreateUserWithRandomPassword_ThenResultIsValid_Async()
        {
            //before
            var userToCreate = GetUserBaseDto();

            //act
            var userCreateResult = await _sut.CreateAsync(userToCreate, BasicUserPassword);

            //then
            userCreateResult.IsValid.Should().Be.True();
            userCreateResult.Result.Should().Not.Be.Null();
        }

        [Fact]
        public void CreateUserWithPassword_ThenResultIsValid()
        {
            //before
            var userToCreate = GetUserBaseDto();

            //act
            var userCreateResult = _sut.Create(userToCreate, BasicUserPassword);

            //then
            userCreateResult.IsValid.Should().Be.True();
            userCreateResult.Result.Should().Not.Be.Null();
        }

        [Fact]
        public async void CreateUserWithPassword_ThenResultIsValid_Async()
        {
            //before
            var userToCreate = GetUserBaseDto();

            //act
            var userCreateResult = await _sut.CreateAsync(userToCreate, BasicUserPassword);

            //then
            userCreateResult.IsValid.Should().Be.True();
            userCreateResult.Result.Should().Not.Be.Null();
        }

        [Fact]
        public void WhenGetUserByEmail_ThenResultIsValid()
        {
            //act
            var userGetResult = _sut.GetByEmail("tkaminski93@gmail.com");

            //then
            userGetResult.IsValid.Should().Be.True();
            userGetResult.Result.Should().Not.Be.Null();
        }


        [Fact]
        public void WhenGetUserByEmail_ThenResultIsNotValid()
        {
            //act
            var userGetResult = _sut.GetByEmail("tkaminadasdasdski93@gmail.com");

            //then
            userGetResult.IsValid.Should().Be.False();
            userGetResult.Result.Should().Be.Null();
        }

        [Fact]
        public async void WhenGetUserByEmail_ThenResultIsValid_Async()
        {
            //act
            var userGetResult = await _sut.GetByEmailAsync("tkaminski93@gmail.com");

            //then
            userGetResult.IsValid.Should().Be.True();
            userGetResult.Result.Should().Not.Be.Null();
        }


        [Fact]
        public async void WhenGetUserByEmail_ThenResultIsNotValid_Async()
        {
            //act
            var userGetResult = await _sut.GetByEmailAsync("tkaminadasdasdski93@gmail.com");

            //then
            userGetResult.IsValid.Should().Be.False();
            userGetResult.Result.Should().Be.Null();
        }

        [Fact]
        public async void WhenUserChangeEmail_WithValidPassword_ThenEmailIsChanged()
        {
            InitContext();
            var studentDto = GetUserBaseDto();
            _sut.Create(studentDto, BasicUserPassword);

            //act
            InitContext();
            var result = await _sut.ChangeEmailAsync(studentDto.Email, "asddasasdasdads@adsasdsa.pl", BasicUserPassword);

            //then
            result.Result.Should().Not.Be.Null();
            result.IsValid.Should().Be.True();
            result.Result.Email.Should().Be.EqualTo("asddasasdasdads@adsasdsa.pl");
        }

        [Fact]
        public async void WhenUserChangeEmail_WithWrongPassword_ThenEmailIsNotChanged()
        {
            //act
            var result = await _sut.ChangeEmailAsync("tkaminski93@gmail.com", "asddasasdasdads@adsasdsa.pl", "wrongpassword");

            //then
            result.Result.Should().Be.Null();
            result.IsValid.Should().Be.False();
        }

        [Fact]
        public async void WhenGetChargesByEmail_ThenResultIsValid_Async()
        {
            var user = GetUserBaseDto();
            //act
            var userGetResult = await _sut.GetChargesAsync("tkaminski93@gmail.com", user.PasswordHash);

            //then
            userGetResult.IsValid.Should().Be.True();
            userGetResult.Result.Should().Be.EqualTo(300);

        }

        [Fact]
        public async void WhenAddChargesByEmail_ThenResultIsValid_Async()
        {
            InitContext();
            //act
            var userGetResult = await _sut.AddChargesAsync("tkaminski93@gmail.com", 25);

            //then
            userGetResult.IsValid.Should().Be.True();
            userGetResult.Result.Should().Be.EqualTo(325);
        }

        [Fact]
        public async void WhenCredentialsProvided_LoginSuccessfull()
        {
            //act
            var result = await _sut.LoginAsync("tkaminski93@gmail.com", BasicUserPassword);

            //then
            result.Should().Not.Be.Null();
            result.IsValid.Should().Be.True();
            result.Result.Email.Should().Be.EqualTo("tkaminski93@gmail.com");

        }

        [Fact]
        public async void WhenCredentialsProvided_LoginUnSuccessfull()
        {
            //act
            var result = await _sut.LoginAsync("tkaminski93@gmail.com", "dsadsasdaasdsda");

            //then
            result.Should().Not.Be.Null();
            result.IsValid.Should().Be.False();
            result.Result.Should().Be.Null();
        }

        [Fact]
        public async void WhenAccountEmailProvided_ThenCheckIfAccountExists_Exists()
        {
            //act
            var result = await _sut.AccountExistsAsync("tkaminski93@gmail.com");

            //then
            result.Should().Not.Be.Null();
            result.IsValid.Should().Be.True();
            result.Result.Should().Be.True();
        }

        [Fact]
        public async void WhenAccountEmailProvided_ThenCheckIfAccountExists_NotExists()
        {
            var result = await _sut.AccountExistsAsync("tkamindsadasdasski93@gmail.com");

            //then
            result.Should().Not.Be.Null();
            result.Result.Should().Be.False();
        }

        [Fact]
        public async void WhenAccountEmailProvided_ThenCheckIfAdmin_IsNot()
        {
            //act
            var result = await _sut.IsAdmin("tkaminski93@gmail.com");

            //then
            result.Should().Not.Be.Null();
            result.IsValid.Should().Be.True();
            result.Result.Should().Be.False();
        }

        [Fact]
        public async void WhenCredentialsEdited_ThenResultIsValid()
        {
            InitContext();
            var studentDto = GetUserBaseDto();
            _sut.Create(studentDto, BasicUserPassword);

            //act
            InitContext();
            studentDto.LastName = "Changed";
            studentDto.Name = "Changed";
            var userGetResult = await _sut.EditStudentInitialsAsync(studentDto);

            //then
            userGetResult.IsValid.Should().Be.True();
            userGetResult.Result.Name.Should().Be.EqualTo("Changed");
            userGetResult.Result.LastName.Should().Be.EqualTo("Changed");
        }

        [Fact]
        public async void WhenTransferCharges_ThenResultIsValid_OnBothSides()
        {
            InitContext();
            var studentDto1 = GetUserBaseDto();
            studentDto1.Email = "student1dto@wp.pl";
            var studentDto2 = GetUserBaseDto();
            studentDto2.Email = "student2dto@wp.pl";
            await _sut.CreateAsync(studentDto1, "pass");
            await _sut.CreateAsync(studentDto2, "pass");

            //act
            InitContext();
            var userTransferResult = await _sut.TransferCharges("student1dto@wp.pl", "student2dto@wp.pl", 150, "pass");

            //then
            userTransferResult.IsValid.Should().Be.True();
            userTransferResult.Result.Should().Be.EqualTo(studentDto1.Charges - 150);
        }

        [Fact]
        public async void GenerateResetPasswordToken_ThenTryToChangePasswordWithThatToken_PasswordIsChanged()
        {
            InitContext();
            var studentDto = GetUserBaseDto();
            _sut.Create(studentDto, BasicUserPassword);
            //before

            InitContext();
            var passwordChangeToken = await _sut.GetPasswordChangeTokenAsync(studentDto.Email);

            //act
            InitContext();
            var resetPasswordResult =
                await _sut.ResetPasswordAsync(System.Net.WebUtility.UrlDecode(passwordChangeToken.SecondResult), "NewPassword123");

            //then
            resetPasswordResult.IsValid.Should().Be.True();
            resetPasswordResult.Result.Should().Not.Be.Null();

            InitContext();
            var validLoginResult = await _sut.LoginAsync(studentDto.Email, "NewPassword123");
            validLoginResult.IsValid.Should().Be.True();
            validLoginResult.Result.Should().Not.Be.Null();

            InitContext();
            var oldPasswordLoginResult = await _sut.LoginAsync(studentDto.Email, BasicUserPassword);
            oldPasswordLoginResult.IsValid.Should().Be.False();
            oldPasswordLoginResult.Result.Should().Be.Null();
        }

        [Fact]
        public async void WhenEmailAndHashIsGiven_ThenLoginIsChecked_ResultIsValid()
        {
            InitContext();
            var studentDto = GetUserBaseDto();
            _sut.Create(studentDto, BasicUserPassword);

            //act
            InitContext();
            var checkLoginResult = await _sut.CheckLoginAsync(studentDto.Email, studentDto.PasswordHash);

            //then
            checkLoginResult.IsValid.Should().Be.True();
            checkLoginResult.Result.Should().Not.Be.Null();
        }

        [Fact]
        public async void WhenEmailAndHashIsGiven_ThenLoginIsChecked_ResultIsNotValid()
        {
            InitContext();
            var studentDto = GetUserBaseDto();
            _sut.Create(studentDto, BasicUserPassword);

            //act
            InitContext();
            var checkLoginResult = await _sut.CheckLoginAsync(studentDto.Email, "dsasad'sda'sad'sad");

            //then
            checkLoginResult.IsValid.Should().Be.False();
            checkLoginResult.Result.Should().Be.Null();
        }

        [Fact]
        public async void WhenEmailAndHashIsGiven_ThenHashIsChecked_ResultIsValid()
        {
            InitContext();
            var studentDto = GetUserBaseDto();
            _sut.Create(studentDto, BasicUserPassword);

            //act
            InitContext();
            var checkLoginResult = await _sut.CheckHashAsync(studentDto.Email, studentDto.PasswordHash);

            //then
            checkLoginResult.IsValid.Should().Be.True();
            checkLoginResult.Result.Should().Be.True();
        }

        [Fact]
        public async void WhenEmailAndHashIsGiven_ThenHashIsChecked_ResultIsNotValid()
        {
            InitContext();
            var studentDto = GetUserBaseDto();
            _sut.Create(studentDto, BasicUserPassword);

            //act
            InitContext();
            var checkLoginResult = await _sut.CheckHashAsync(studentDto.Email, "dsasad'sda'sad'sad");

            //then
            checkLoginResult.IsValid.Should().Be.False();
            checkLoginResult.Result.Should().Be.False();
        }
    }
}
