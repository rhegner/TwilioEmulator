using System;
using System.Collections.Generic;
using TwilioLogic.TwilioModels;

namespace TwilioEmulator.TwilioModels
{
    public class NotificationsPage : ResourcePage<Notification>
    {
        public NotificationsPage(IReadOnlyList<Notification> notifications, bool hasMore, Uri currentUrl)
            : base(notifications, hasMore, currentUrl)
        { }

        public IReadOnlyList<Notification> Notifications { get => Resources; }
    }
}
