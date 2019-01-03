using System;

namespace TwilioLogic.Models
{
    public class ApiCall
    {

        public Guid ApiCallId { get; internal set; }

        public string Sid { get; internal set; }

        public ApiCallDirection Direction { get; internal set; }

        public ApiCallType Type { get; internal set; }

        public string HttpMethod { get; internal set; }

        public Uri Url { get; internal set; }

        public DateTime RequestTimestamp { get; internal set; }

        public string RequestContentType { get; internal set; }

        public string RequestContent { get; internal set; }

        public DateTime? ResponseTimestamp { get; internal set; }

        public string ResponseContentType { get; internal set; }

        public string ResponseContent { get; internal set; }

        public int ResponseStatusCode { get; internal set; }



        public string CurrentActivity { get; internal set; }

    }
}
