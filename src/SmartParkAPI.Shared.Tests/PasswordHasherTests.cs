using SharpTestsEx;
using SmartParkAPI.Shared.Helpers;
using Xunit;

namespace SmartParkAPI.Shared.Tests
{
    public class PasswordHasherTests
    {
        private readonly PasswordHasher _sut;
        private const string ValidPassword = "Password123";
        private const string NotValidPassword = "Password12345678";
        private const char SplitCharacter = ':';

        public PasswordHasherTests()
        {
            _sut = new PasswordHasher();
        }

        [Fact]
        public void WhenEncryptPassword_ReturnHashAndSalt()
        {
            //Act
            var encrypted = _sut.CreateHash(ValidPassword);

            //Then
            encrypted.Should().Not.Be.Null();
            encrypted.Hash.Should().Not.Be.Empty();
            encrypted.Salt.Should().Not.Be.Empty();
        }

        [Fact]
        public void WhenEncryptPassword_ThenValidateIt_ResultIsValid()
        {
            //Act
            var encrypted = _sut.CreateHash(ValidPassword);
            var validationResult = _sut.ValidatePassword(ValidPassword, encrypted.Hash, encrypted.Salt);

            //Then
            encrypted.Should().Not.Be.Null();
            encrypted.Hash.Should().Not.Be.Empty();
            encrypted.Salt.Should().Not.Be.Empty();
            validationResult.Should().Be.True();
        }

        [Fact]
        public void WhenEncryptPassword_ThenValidateIt_ResultIsNotValid()
        {
            //Act
            var encrypted = _sut.CreateHash(ValidPassword);
            var validationResult = _sut.ValidatePassword(NotValidPassword, encrypted.Hash, encrypted.Salt);


            //Then
            encrypted.Should().Not.Be.Null();
            encrypted.Hash.Should().Not.Be.Empty();
            encrypted.Salt.Should().Not.Be.Empty();
            validationResult.Should().Be.False();
        }
    }
}
