using System;

namespace SWGame.Exceptions.ValidationExceptions
{
    public class WrongLoginException : Exception
    {
        public WrongLoginException(string message) :
            base(message)
        { }
    }
}
