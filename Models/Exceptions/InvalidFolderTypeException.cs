using System;

namespace Models.Exceptions
{
    public class InvalidFolderTypeException : Exception
    {
        public InvalidFolderTypeException(string message)
            : base($"{message} type does not exist!")
        {
        }
    }
}
