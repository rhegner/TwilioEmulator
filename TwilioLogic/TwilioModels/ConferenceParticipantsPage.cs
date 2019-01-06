using System;
using System.Collections.Generic;

namespace TwilioLogic.TwilioModels
{
    public class ConferenceParticipantsPage : ResourcePage<ConferenceParticipant>
    {

        public ConferenceParticipantsPage(List<ConferenceParticipant> conferenceParticipants, bool hasMore, Uri currentUrl)
            : base(conferenceParticipants, hasMore, currentUrl)
        { }

        public List<ConferenceParticipant> Participants { get => Resources; }
    }
}
