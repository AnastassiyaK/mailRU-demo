using System;

namespace PageObjects.Exceptions
{
    public class DraftNotFoundException : Exception
    {
        public DraftNotFoundException(string message)
               : base($"{message} draft does not exist! Nothing to send!")
        {
        }
    }
}
