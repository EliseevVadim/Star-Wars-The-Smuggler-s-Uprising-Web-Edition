using SWGame.Exceptions.ValidationExceptions;
using System;

namespace SWGame.Management.Validators
{
    public class PasswordValidator : RegistrationValidator
    {
        private bool _passwordConfirmed;
        public PasswordValidator(string text, bool passwordConfirmed)
        {
            _text = text;
            _passwordConfirmed = passwordConfirmed;
        }
        public override void Validate()
        {
            if (String.IsNullOrWhiteSpace(_text) || _text.Length < MinimalPasswordLength)
            {
                throw new WrongPasswordException($"������ �� ������ ���� ������ {MinimalPasswordLength} ��������");
            }
            if (!_passwordConfirmed)
            {
                throw new WrongPasswordException($"������ �� ���������");
            }
        }
    }
}
