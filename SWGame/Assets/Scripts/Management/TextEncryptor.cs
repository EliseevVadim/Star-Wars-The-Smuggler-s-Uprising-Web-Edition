using System;
using System.Security.Cryptography;
using System.Text;

namespace SWGame.Management
{
    public class TextEncryptor
    {
        private string _line;

        public TextEncryptor(string line)
        {
            _line = line;
        }
        public string GetSha1EncryptedLine()
        {
            var sha1 = SHA1.Create();
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(_line));
            return Convert.ToBase64String(hash);
        }
    }
}
