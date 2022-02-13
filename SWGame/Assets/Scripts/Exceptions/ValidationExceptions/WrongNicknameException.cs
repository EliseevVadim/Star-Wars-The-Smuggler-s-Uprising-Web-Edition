using System;

namespace SWGame.Exceptions.ValidationExceptions
{
    public class WrongNicknameException : Exception
    {
        public WrongNicknameException(string message) :
            base(message)
        { }
    }
}
