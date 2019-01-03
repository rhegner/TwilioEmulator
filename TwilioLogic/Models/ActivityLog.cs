using System;

namespace TwilioLogic.Models
{
    public class ActivityLog
    {

        public Guid ActivityLogId { get; internal set; }

        public string Sid { get; internal set; }

        public DateTime Timestamp { get; internal set; }

        public string Message { get; internal set; }
    }
}
