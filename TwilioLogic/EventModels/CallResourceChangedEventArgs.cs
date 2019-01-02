using System;
using TwilioLogic.Models;

namespace TwilioLogic.EventModels
{
    public class CallResourceChangedEventArgs : EventArgs
    {
        public CallResourceChangedEventArgs(CallResource callResource, bool isNew = false)
        {
            CallResource = callResource;
            IsNew = isNew;
        }

        public CallResource CallResource { get; }
        public bool IsNew { get; }
    }
}
