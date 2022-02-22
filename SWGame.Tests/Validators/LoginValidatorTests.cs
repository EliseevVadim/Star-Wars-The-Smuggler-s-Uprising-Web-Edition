using NUnit.Framework;
using SWGame.Exceptions.ValidationExceptions;
using SWGame.Management.Validators;

namespace SWGame.Tests.Validators
{
    [TestFixture]
    public class LoginValidatorTests
    {
        [Test]
        public void TestNullLogin()
        {
            RegistrationValidator validator = new LoginValidator(null);
            Assert.Throws<WrongLoginException>(() => validator.Validate());
        }

        [Test]
        public void TestWhiteSpacesLogin()
        {
            RegistrationValidator validator = new LoginValidator("                                   ");
            Assert.Throws<WrongLoginException>(() => validator.Validate());
        }

        [Test]
        public void TestEmptyLogin()
        {
            RegistrationValidator validator = new LoginValidator(string.Empty);
            Assert.Throws<WrongLoginException>(() => validator.Validate());
        }

        [Test]
        public void TestCorrectLogin()
        {
            RegistrationValidator validator = new LoginValidator("Администратор");
            Assert.DoesNotThrow(() => validator.Validate());
        }
    }
}
