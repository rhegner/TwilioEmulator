using System;
using System.Collections.Generic;
using TwilioLogic.TwilioModels;

namespace TwilioEmulator.TwilioModels
{
    public class ConferenceParticipantsPage : ResourcePage<ConferenceParticipant>
    {

        public ConferenceParticipantsPage(IReadOnlyList<ConferenceParticipant> conferenceParticipants, bool hasMore, Uri currentUrl)
            : base(conferenceParticipants, hasMore, currentUrl)
        { }

        public IReadOnlyList<ConferenceParticipant> Participants { get => Resources; }
    }
}
