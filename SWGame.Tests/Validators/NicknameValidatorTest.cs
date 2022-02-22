using NUnit.Framework;
using SWGame.Exceptions.ValidationExceptions;
using SWGame.Management.Validators;

namespace SWGame.Tests.Validators
{
    [TestFixture]
    public class NicknameValidatorTest
    {
        [Test]
        public void TestNullNickname()
        {
            RegistrationValidator validator = new NicknameValidator(null);
            Assert.Throws<WrongNicknameException>(() => validator.Validate());
        }

        [Test]
        public void TestShortNickname()
        {
            RegistrationValidator validator = new NicknameValidator("Y0");
            Assert.Throws<WrongNicknameException>(() => validator.Validate());
        }

        [Test]
        public void TestCorrectNickname()
        {
            RegistrationValidator validator = new NicknameValidator("Администратор");
            Assert.DoesNotThrow(() => validator.Validate());
        }

        [Test]
        public void TestEmptyNickname()
        {
            RegistrationValidator validator = new NicknameValidator(string.Empty);
            Assert.Throws<WrongNicknameException>(() => validator.Validate());
        }

        [Test]
        public void TestWhiteSpacesNickname()
        {
            RegistrationValidator validator = new NicknameValidator("                                   ");
            Assert.Throws<WrongNicknameException>(() => validator.Validate());
        }
    }
}
