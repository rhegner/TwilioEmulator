using System;
using TwilioLogic.Interfaces;
using TwilioLogic.Utils;

namespace TwilioLogic.TwilioModels
{
    public class Alert : IResource
    {

        public Alert(Uri baseUri)
        {
            Url = $"{baseUri.GetComponents(UriComponents.SchemeAndServer, UriFormat.SafeUnescaped)}/v1/Alerts/{Sid}";
        }

        public string AccountSid { get; internal set; } = TwilioUtils.CreateSid("AC");

        public string AlertText { get; internal set; } = "";

        public string ApiVersion { get; internal set; } = "2010-04-01";

        public DateTime DateCreated { get; internal set; } = DateTime.UtcNow;

        public DateTime DateGenerated { get; internal set; } = DateTime.UtcNow;

        public DateTime DateUpdated { get; internal set; } = DateTime.UtcNow;

        public string ErrorCode { get; internal set; } = "";

        public string LogLevel { get; internal set; } = "debug";

        public string MoreInfo { get; internal set; } = "";

        public string RequestHeaders { get; internal set; } = "";

        public string RequestMethod { get; internal set; } = "";

        public string RequestUrl { get; internal set; } = "";

        public string RequestVariables { get; internal set; } = "";

        public string ResourceSid { get; internal set; } = "";

        public string ResponseBody { get; internal set; } = "";

        public string ResponseHeaders { get; internal set; } = "";

        public string Sid { get; internal set; } = TwilioUtils.CreateSid("NO");

        public string Url { get; }




        public string GetSid() => Sid;

        public string GetTopLevelSid() => ResourceSid;

    }
}
