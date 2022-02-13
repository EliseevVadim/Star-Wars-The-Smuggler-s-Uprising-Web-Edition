using SWGame.Exceptions.ValidationExceptions;
using System;

namespace SWGame.Management.Validators
{
    public class NicknameValidator : RegistrationValidator
    {
        public NicknameValidator(string text)
        {
            _text = text;
        }
        public override void Validate()
        {
            if (String.IsNullOrWhiteSpace(_text) || _text.Length < MinimailNicknameLength)
            {
                throw new WrongNicknameException($"Имя не должно быть короче {MinimailNicknameLength} символов");
            }
            // Проверка на наличие никнейма в БД
            //if (_nickName in DB)
            //{
            // throw new ArgumentException("Никнейм уже занят");
            //}
        }
    }
}
