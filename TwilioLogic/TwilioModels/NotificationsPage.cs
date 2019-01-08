using System;
using System.Collections.Generic;

namespace TwilioLogic.TwilioModels
{
    public class NotificationsPage : ResourcePage<Notification>
    {
        public NotificationsPage(List<Notification> notifications, bool hasMore, Uri currentUrl)
            : base(notifications, hasMore, currentUrl)
        { }

        public List<Notification> Notifications { get => Resources; }
    }
}
