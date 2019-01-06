using System;
using TwilioLogic.Interfaces;
using TwilioLogic.Utils;

namespace TwilioLogic.TwilioModels
{
    public class Conference : IResource
    {
        public string AccountSid { get; internal set; } = TwilioUtils.CreateSid("AC");

        public string ApiVersion { get; internal set; } = "2010-04-01";

        public DateTime DateCreated { get; internal set; } = DateTime.UtcNow;

        public DateTime DateUpdated { get; internal set; } = DateTime.UtcNow;

        public string FriendlyName { get; internal set; } = "";

        public string Sid { get; internal set; } = TwilioUtils.CreateSid("CF");

        public string Region { get; internal set; } = "us1";

        public string Status { get; internal set; } = "init";

        public ConferenceSubResources SubresourceUris { get => new ConferenceSubResources(this); }

        public string Uri { get => $"/{ApiVersion}/Accounts/{AccountSid}/Conferences/{Sid}.json"; }


        // non-twilio properties

        public int NumberOfParticipants { get; internal set; } = 0;


        // IResouce implementation


        public string GetSid() => Sid;
        public string GetTopLevelSid() => Sid;
    }

    public class ConferenceSubResources
    {
        private readonly Conference ConferenceResource;

        public ConferenceSubResources(Conference conferenceResource)
        {
            ConferenceResource = conferenceResource;
        }

        public string Participants { get => $"/{ConferenceResource.ApiVersion}/Accounts/{ConferenceResource.AccountSid}/Conferences/{ConferenceResource.Sid}/Participants.json"; }
        public string Recordings { get => $"/{ConferenceResource.ApiVersion}/Accounts/{ConferenceResource.AccountSid}/Conferences/{ConferenceResource.Sid}/Recordings.json"; }
    }
}
