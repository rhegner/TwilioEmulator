using System;
using TwilioLogic.Interfaces;

namespace TwilioLogic.TwilioModels
{

    /// <summary>
    /// https://www.twilio.com/docs/usage/api/notifications-deprecated
    /// </summary>
    public class Notification : IResource
    {
        private readonly Alert Alert;

        public Notification(Alert alert)
        {
            Alert = alert;
        }

        public string AccountSid { get => Alert.AccountSid; }

        public string ApiVersion { get => Alert.ApiVersion; }

        public string CallSid { get => Alert.ResourceSid; }

        public DateTime DateCreated { get => Alert.DateCreated; }

        public DateTime DateUpdated { get => Alert.DateUpdated; }

        public string ErrorCode { get => Alert.ErrorCode; }

        public int Log { get => LogLevelToLog(Alert.LogLevel); }

        public DateTime MessageDate { get => Alert.DateGenerated; }

        public string MessageText { get => Alert.AlertText; }

        public string MoreInfo { get => Alert.MoreInfo; }

        public string RequestMethod { get => Alert.RequestMethod; }

        public string RequestUrl { get => Alert.RequestUrl; }

        public string RequestVariables { get => Alert.RequestVariables; }

        public string ResponseBody { get => Alert.ResponseBody; }

        public string ResponseHeaders { get => Alert.ResponseHeaders; }

        public string Sid { get => Alert.Sid; }

        public string Uri { get => $"/{ApiVersion}/Accounts/{AccountSid}/Notifications/{Sid}.json"; }



        public string GetSid() => Alert.GetSid();

        public string GetTopLevelSid() => Alert.GetTopLevelSid();



        public static int LogLevelToLog(string logLevel)
        {
            switch (logLevel)
            {
                case "error":
                    return 0;
                case "warning":
                    return 1;
                case "notice":
                    return 2;       // this is a custom addition, not documented in the twilio documentation
                case "debug":
                    return 3;       // this is a custom addition, not documented in the twilio documentation
                default:
                    throw new NotSupportedException($"Unknown log level {logLevel}");
            }
        }

        public static string LogToLogLevel(int log)
        {
            switch (log)
            {
                case 0:
                    return "error";
                case 1:
                    return "warning";
                case 2:             // this is a custom addition, not documented in the twilio documentation
                    return "notice";
                case 3:             // this is a custom addition, not documented in the twilio documentation
                    return "debug"; 
                default:
                    throw new NotSupportedException($"Unknown log level {log}");
            }
        }

    }
}
