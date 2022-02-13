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
                throw new WrongPasswordException($"Пароль не должен быть короче {MinimalPasswordLength} символов");
            }
            if (!_passwordConfirmed)
            {
                throw new WrongPasswordException($"Пароли не совпадают");
            }
        }
    }
}
