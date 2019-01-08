using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using TwilioLogic.Exceptions;
using TwilioLogic.RepositoryInterfaces;
using TwilioLogic.TwilioModels;

namespace TwilioMemoryRepositories
{
    public class AlertRepository : IAlertRepository
    {
        private readonly List<Alert> Alerts = new List<Alert>();
        private readonly object AlertsLock = new object();

        public Task CreateAlert(Alert alert)
        {
            if (string.IsNullOrEmpty(alert.Sid))
                throw new ArgumentException("No alert sid provided");
            lock (AlertsLock)
            {
                if (Alerts.Any(a => a.Sid.Equals(alert.Sid, StringComparison.InvariantCultureIgnoreCase)))
                    throw new InvalidOperationException($"Alert with sid {alert.Sid} does already exist");
                Alerts.Add(alert);
            }
            return Task.CompletedTask;
        }

        public Task<Alert> GetAlert(string alertSid)
        {
            if (string.IsNullOrEmpty(alertSid))
                throw new ArgumentException("No alertSid provided", nameof(alertSid));
            lock (AlertsLock)
            {
                var alert = Alerts.SingleOrDefault(a => a.Sid == alertSid);
                if (alert == null)
                    throw new NotFoundException(alertSid);
                return Task.FromResult(alert);
            }
        }

        public Task<NotificationsPage> GetNotificationsPage(Uri url)
        {
            string callSidFilter = null;

            var pathMatch = Regex.Match(url.GetComponents(UriComponents.Path, UriFormat.SafeUnescaped), "^\\d{4}-\\d{2}-\\d{2}\\/Accounts\\/AC\\w{32}\\/Calls\\/(?<callId>CA\\w{32})\\/Notifications\\.json$"))
            if (pathMatch.Success)
            {

            }
            if (!pathMatch.Success)
                throw new ArgumentException("Url does not match regex", nameof(url));
            var callSidFilter = pathMatch.Groups["callId"].Value;

            var queryParams = HttpUtility.ParseQueryString(url.Query);

            var logFilter = queryParams.GetInt("Log");
            var messageDateFilter = queryParams.GetDateTime("MessageDate");
            var messageDateBeforeFilter = queryParams.GetDateTime("MessageDate<");
            var messageDateAfterFilter = queryParams.GetDateTime("MessageDate>");
            var page = queryParams.GetPage();
            var pageSize = queryParams.GetPageSize();
            var pageToken = queryParams.GetPageToken();

            lock (AlertsLock)
            {
                IEnumerable<Alert> query = Alerts;

                if (!string.IsNullOrEmpty(callSidFilter))
                    query = query.Where(a => a.ResourceSid == callSidFilter);
                if (logFilter.HasValue)
                    query = query.Where(a => a.LogLevel == Notification.LogToLogLevel(logFilter.Value));
                if (messageDateFilter.HasValue)
                    query = query.Where(a => a.DateGenerated >= messageDateFilter.Value.Date && a.DateGenerated < messageDateFilter.Value.AddDays(1).Date);
                if (messageDateBeforeFilter.HasValue)
                    query = query.Where(a => a.DateGenerated < messageDateBeforeFilter.Value.Date);
                if (messageDateAfterFilter.HasValue)
                    query = query.Where(a => a.DateGenerated >= messageDateAfterFilter.Value.Date);

                query = query.OrderBy(c => c.DateGenerated);

                if (page == 0 && string.IsNullOrEmpty(pageToken))
                    query = query.Take(pageSize);
                else if (page == 0 && !string.IsNullOrEmpty(pageToken))
                    query = query.TakeWhile(c => c.Sid != pageToken).TakeLast(pageSize);
                else if (page > 0 && !string.IsNullOrEmpty(pageToken))
                    query = query.SkipWhile(c => c.Sid != pageToken).Skip(1).Take(pageSize);
                else
                    throw new Exception($"Invalid paging with page={page}, pageSize={pageSize}, pageToken={pageToken}");

                var items = query.ToList();
                var hasMore = query.Take(1) != null;
                var callsPage = new NotificationsPage(items.Select (a => new Notification(a)).ToList(), hasMore, url);
                return Task.FromResult(callsPage);
            }
        }

        public Task DeleteAlert(string alertSid)
        {
            if (string.IsNullOrEmpty(alertSid))
                throw new ArgumentException("No alertSid provided", nameof(alertSid));
            lock (AlertsLock)
            {
                var index = Alerts.FindIndex(a => a.Sid.Equals(alertSid, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                    throw new NotFoundException(alertSid);
                else
                    Alerts.RemoveAt(index);
            }
            return Task.CompletedTask;
        }
    }
}
