using System;
using System.Collections.Generic;
using TwilioLogic.Utils;

namespace TwilioLogic.Models
{
    public class ConferenceResource
    {
        public string AccountSid { get; internal set; } = TwilioUtils.CreateSid("AC");

        public string ApiVersion { get; internal set; } = "2008-08-01";

        public DateTime DateCreated { get; internal set; } = DateTime.UtcNow;

        public DateTime DateUpdated { get; internal set; } = DateTime.UtcNow;

        public string FriendlyName { get; internal set; } = "";

        public string Sid { get; internal set; } = TwilioUtils.CreateSid("CF");

        public string Region { get; internal set; } = "us1";

        public string Status { get; internal set; } = "init";

        public Dictionary<string, string> SubresourceUris { get; internal set; } = new Dictionary<string, string>();

        public string Uri { get => $"/2010-04-01/Accounts/{AccountSid}/Conferences/{Sid}.json"; }




        public int NumberOfParticipants { get; internal set; } = 0;

    }
}
