using SWGame.Exceptions.ValidationExceptions;
using System;

namespace SWGame.Management.Validators
{
    public class LoginValidator : RegistrationValidator
    {
        public LoginValidator(string text)
        {
            _text = text;
        }
        public override void Validate()
        {
            if (String.IsNullOrWhiteSpace(_text))
            {
                throw new WrongLoginException("Логин не должен быть пустым");
            }
        }
    }
}
