using System;

namespace Core.Exceptions
{
    public class ErrorDriverInitializationException : Exception
    {
        public ErrorDriverInitializationException(string message)
            : base($"Browser with name {message} does not exist")
        {
        }
    }
}
