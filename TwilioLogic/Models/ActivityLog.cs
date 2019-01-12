using System;
using TwilioLogic.Interfaces;

namespace TwilioLogic.Models
{
    public class ActivityLog : IResource
    {

        public string Sid { get; internal set; }

        public string ResourceSid { get; internal set; }

        public DateTime Timestamp { get; internal set; }

        public string Message { get; internal set; }



        public string GetSid() => Sid;

        public string GetTopLevelSid() => ResourceSid;

    }
}
