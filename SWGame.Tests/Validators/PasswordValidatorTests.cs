using NUnit.Framework;
using SWGame.Exceptions.ValidationExceptions;
using SWGame.Management.Validators;

namespace SWGame.Tests.Validators
{
    [TestFixture]
    public class PasswordValidatorTests
    {
        [Test]
        public void TestCorrectCorrespondingPasswords()
        {
            // Arrange
            RegistrationValidator validator = new PasswordValidator("12345678", true);
            // Act and assert
            Assert.DoesNotThrow(() => validator.Validate());
        }

        [Test]
        public void TestUnconfirmedPasswords()
        {
            // Arrange
            RegistrationValidator validator = new PasswordValidator("12345678", false);
            // Act and assert
            Assert.Throws<WrongPasswordException>(() => validator.Validate());
        }

        [Test]
        public void TestNullAndConfirmedPassword()
        {
            // Arrange
            RegistrationValidator validator = new PasswordValidator(null, true);
            // Act and assert
            Assert.Throws<WrongPasswordException>(() => validator.Validate());
        }

        [Test]
        public void TestIncorrectConfirmedPasswords()
        {
            // Arrange
            RegistrationValidator validator = new PasswordValidator("abcd", true);
            // Act and assert
            Assert.Throws<WrongPasswordException>(() => validator.Validate());
        }

        [Test]
        public void TestWhiteSpacesPassword()
        {
            // Arrange
            RegistrationValidator validator = new PasswordValidator("             ", true);
            // Act and assert
            Assert.Throws<WrongPasswordException>(() => validator.Validate());
        }
    }
}
