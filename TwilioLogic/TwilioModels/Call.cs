using System;
using TwilioLogic.Interfaces;
using TwilioLogic.Utils;

namespace TwilioLogic.TwilioModels
{
    public class Call : IResource
    {
        public string AccountSid { get; internal set; } = TwilioUtils.CreateSid("AC");

        public string Annotation { get; internal set; } = null;

        public string AnsweredBy { get; internal set; } = null;

        public string ApiVersion { get; internal set; } = "2010-04-01";

        public string CallerName { get; internal set; } = null;

        public DateTime DateCreated { get; internal set; } = DateTime.UtcNow;

        public DateTime DateUpdated { get; internal set; } = DateTime.UtcNow;

        public string Direction { get; internal set; } = "inbound";

        public string Duration { get; internal set; } = null;

        public DateTime? EndTime { get; internal set; } = null;

        public string ForwardedFrom { get; internal set; } = null;

        public string From { get; internal set; } = "";

        public string FromFormatted { get => TwilioUtils.FormatNumber(From); }

        public string GroupSid { get; internal set; } = null;

        public string ParentCallSid { get; internal set; } = null;

        public string PhoneNumberSid { get; internal set; } = TwilioUtils.CreateSid("PN");

        public decimal Price { get; internal set; } = 0;

        public string PriceUnit { get; internal set; } = "USD";

        public string Sid { get; internal set; } = TwilioUtils.CreateSid("CA");

        public DateTime? StartTime { get; internal set; } = null;

        public string Status { get; internal set; } = "queued";

        public CallSubResources SubresourceUris { get => new CallSubResources(this); }

        public string To { get; internal set; } = "";

        public string ToFormatted { get => TwilioUtils.FormatNumber(To); }

        public string Uri { get => $"/{ApiVersion}/Accounts/{AccountSid}/Calls/{Sid}.json"; }




        // IResource implementation

        public string GetSid() => Sid;

        public string GetTopLevelSid() => Sid;
    }

    public class CallSubResources
    {
        private readonly Call Call;

        public CallSubResources(Call call)
        {
            Call = call;
        }

        public string Notifications { get => $"/{Call.ApiVersion}/Accounts/{Call.AccountSid}/Calls/{Call.Sid}/Notifications.json"; }
        public string Recordings { get => $"/{Call.ApiVersion}/Accounts/{Call.AccountSid}/Calls/{Call.Sid}/Recordings.json"; }
        public string Feedback { get => $"/{Call.ApiVersion}/Accounts/{Call.AccountSid}/Calls/{Call.Sid}/Feedback.json"; }
        public string FeedbackSummaries { get => $"/{Call.ApiVersion}/Accounts/{Call.AccountSid}/Calls/FeedbackSummary.json"; }
    }
}
