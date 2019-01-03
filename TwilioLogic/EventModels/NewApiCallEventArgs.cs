using System;
using TwilioLogic.Models;

namespace TwilioLogic.EventModels
{
    public class NewApiCallEventArgs : EventArgs
    {
        public NewApiCallEventArgs(ApiCall apiCall)
        {
            ApiCall = apiCall;
        }

        public ApiCall ApiCall { get; }
    }
}
