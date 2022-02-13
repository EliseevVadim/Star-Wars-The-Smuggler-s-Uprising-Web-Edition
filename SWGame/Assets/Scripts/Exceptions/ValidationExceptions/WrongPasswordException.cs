using System;

namespace SWGame.Exceptions.ValidationExceptions
{
    public class WrongPasswordException : Exception
    {
        public WrongPasswordException(string message) :
            base(message)
        { }
    }
}
