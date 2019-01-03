using System;
using TwilioLogic.Models;

namespace TwilioLogic.EventModels
{
    public class ConferenceResourceChangedEventArgs : EventArgs
    {
        public ConferenceResourceChangedEventArgs(ConferenceResource conferenceResource)
        {
            ConferenceResource = conferenceResource;
        }

        public ConferenceResource ConferenceResource { get; }
    }
}
