using System;

namespace TwilioLogic.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string sid)
            : base($"Resource with sid={sid} not found!")
        { }
    }
}
