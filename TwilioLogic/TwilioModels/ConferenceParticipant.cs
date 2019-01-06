using System;
using TwilioLogic.Interfaces;
using TwilioLogic.Utils;

namespace TwilioLogic.TwilioModels
{
    public class ConferenceParticipant : IResource
    {

        internal ConferenceParticipant(string apiVersion)
        {
            ApiVersion = apiVersion;
        }



        public string AccountSid { get; internal set; } = TwilioUtils.CreateSid("AC");

        public string CallSid { get; internal set; }

        public string ConferenceSid { get; internal set; }

        public DateTime DateCreated { get; internal set; } = DateTime.UtcNow;

        public DateTime DateUpdated { get; internal set; } = DateTime.UtcNow;

        public bool EndConferenceOnExit { get; internal set; } = false;

        public bool Hold { get; internal set; } = false;

        public bool Muted { get; internal set; } = false;

        public bool StartConferenceOnEnter { get; internal set; } = true;

        public string Status { get; internal set; } = "queued";

        public string Uri { get => $"/{ApiVersion}/Accounts/{AccountSid}/Conferences/{ConferenceSid}/Participants/{CallSid}.json"; }



        private string ApiVersion { get; }



        public string GetSid() => CallSid;

        public string GetTopLevelSid() => ConferenceSid;

    }
}
