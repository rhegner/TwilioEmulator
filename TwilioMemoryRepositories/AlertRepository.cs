using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using TwilioLogic.Exceptions;
using TwilioLogic.Models;
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

        public Task<PageList<Alert>> GetAlerts(string resourceSidFilter, string logLevelFilter, 
            DateTime? messageDateFilter, DateTime? messageDateBeforeFilter, DateTime? messageDateAfterFilter,
            int page, int pageSize, string pageToken)
        {
            lock (AlertsLock)
            {
                IEnumerable<Alert> query = Alerts;

                if (!string.IsNullOrEmpty(resourceSidFilter))
                    query = query.Where(a => a.ResourceSid == resourceSidFilter);
                if (!string.IsNullOrEmpty(logLevelFilter))
                    query = query.Where(a => a.LogLevel == logLevelFilter);
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
                var alertsList = new PageList<Alert>(items, hasMore);
                return Task.FromResult(alertsList);
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

        public Task Clear()
        {
            lock (AlertsLock)
            {
                Alerts.Clear();
            }
            return Task.CompletedTask;
        }

    }
}
