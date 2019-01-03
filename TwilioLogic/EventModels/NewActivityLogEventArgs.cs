using System;
using TwilioLogic.Models;

namespace TwilioLogic.EventModels
{
    public class NewActivityLogEventArgs : EventArgs
    {
        public NewActivityLogEventArgs(ActivityLog activityLog)
        {
            ActivityLog = activityLog;
        }

        public ActivityLog ActivityLog { get; }
    }
}
