using System;

namespace SWGame.Exceptions
{
    class FailedLootException : Exception
    {
        public FailedLootException(string message) :
            base(message)
        { }
    }
}
