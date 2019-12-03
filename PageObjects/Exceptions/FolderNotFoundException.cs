using System;

namespace PageObjects.Exceptions
{
    public class FolderNotFoundException : Exception
    {
        public FolderNotFoundException(string message)
            : base($"{message} folder does not exist!")
        {
        }
    }
}
