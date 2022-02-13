using NUnit.Framework;
using SWGame.Management;
using System;
using System.Security.Cryptography;
using System.Text;

namespace SWGame.Tests.ManagementTools
{
    [TestFixture]
    public class TextEncryptorTest
    {
        [Test]
        public void TestSha1Encryption()
        {
            string target = "line_4_encryption";
            TextEncryptor encryptor = new TextEncryptor(target);
            string output = encryptor.GetSha1EncryptedLine();
            var sha1 = SHA1.Create();
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(target));
            string shouldBe = Convert.ToBase64String(hash);
            Assert.AreEqual(shouldBe, output);
        }
    }
}
