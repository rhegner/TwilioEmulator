using System;
using TwilioLogic.Interfaces;

namespace TwilioLogic.TwilioModels
{
    public class AlertOverview : IResource
    {
        private readonly Alert Alert;

        public AlertOverview(Alert alert)
        {
            Alert = alert;
        }

        public string AccountSid { get => Alert.AccountSid; }

        public string AlertText { get => Alert.AlertText; }

        public string ApiVersion { get => Alert.ApiVersion; }

        public DateTime DateCreated { get => Alert.DateCreated; }

        public DateTime DateGenerated { get => Alert.DateGenerated; }

        public DateTime DateUpdated { get => Alert.DateUpdated; }

        public string ErrorCode { get => Alert.ErrorCode; }

        public string LogLevel { get => Alert.LogLevel; }

        public string MoreInfo { get => Alert.MoreInfo; }

        public string RequestMethod { get => Alert.RequestMethod; }

        public string RequestUrl { get => Alert.RequestUrl; }

        public string ResourceSid { get => Alert.ResourceSid; }

        public string Sid { get => Alert.Sid; }

        public string Url { get => Alert.Url; }



        public string GetSid() => Alert.GetSid();

        public string GetTopLevelSid() => Alert.GetTopLevelSid();
    }
}
