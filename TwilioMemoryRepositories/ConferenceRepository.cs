using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TwilioLogic.Models;
using TwilioLogic.RepositoryInterfaces;
using TwilioLogic.TwilioModels;

namespace TwilioMemoryRepositories
{
    public class ConferenceRepository : IConferenceRepository
    {

        private readonly List<Conference> Conferences = new List<Conference>();
        private readonly object ConferencesLock = new object();

        public Task<Conference> GetOrCreateConference(string friendlyName, Func<string, Conference> factory)
        {
            if (string.IsNullOrEmpty(friendlyName))
                throw new ArgumentException("No friendly name provided", nameof(friendlyName));
            lock (ConferencesLock)
            {
                var conference = Conferences.SingleOrDefault(c => c.FriendlyName.Equals(friendlyName, StringComparison.InvariantCultureIgnoreCase));
                if (conference == null)
                {
                    conference = factory(friendlyName);
                    if (!conference.FriendlyName.Equals(friendlyName, StringComparison.InvariantCultureIgnoreCase))
                        throw new InvalidOperationException("Friendly name does not match");
                    if (string.IsNullOrEmpty(conference.Sid))
                        throw new InvalidOperationException("No conference sid provided for new conference");
                    if (Conferences.Any(c => c.Sid.Equals(conference.Sid, StringComparison.InvariantCultureIgnoreCase)))
                        throw new InvalidOperationException($"Conference with sid {conference.Sid} does already exist");
                    Conferences.Add(conference);
                }
                return Task.FromResult(conference);
            }
        }

        public Task<Conference> GetConference(string sid)
        {
            if (string.IsNullOrEmpty(sid))
                throw new ArgumentException("No conference sid provided", nameof(sid));
            lock (ConferencesLock)
            {
                var conference = Conferences.SingleOrDefault(c => c.Sid == sid);
                if (conference == null)
                    throw new InvalidOperationException($"Conference with sid {sid} not found");
                return Task.FromResult(conference);
            }
        }

        public Task<PageList<Conference>> GetConferences(DateTime? dateCreatedFilter, DateTime? dateCreatedBeforeFilter, DateTime? dateCreatedAfterFilter,
            DateTime? dateUpdatedFilter, DateTime? dateUpdatedBeforeFilter, DateTime? dateUpdatedAfterFilter,
            string friendlyNameFilter, string[] statusFilter,
            int page, int pageSize, string pageToken)
        {
            lock (ConferencesLock)
            {
                IEnumerable<Conference> query = Conferences;

                if (dateCreatedFilter.HasValue)
                    query = query.Where(c => c.DateCreated >= dateCreatedFilter.Value.Date && c.DateCreated < dateCreatedFilter.Value.AddDays(1).Date);
                if (dateCreatedBeforeFilter.HasValue)
                    query = query.Where(c => c.DateCreated < dateCreatedBeforeFilter.Value.Date);
                if (dateCreatedAfterFilter.HasValue)
                    query = query.Where(c => c.DateCreated >= dateCreatedAfterFilter.Value.Date);
                if (dateUpdatedFilter.HasValue)
                    query = query.Where(c => c.DateUpdated >= dateUpdatedFilter.Value.Date && c.DateUpdated < dateUpdatedFilter.Value.AddDays(1).Date);
                if (dateUpdatedBeforeFilter.HasValue)
                    query = query.Where(c => c.DateUpdated < dateUpdatedBeforeFilter.Value.Date);
                if (dateUpdatedAfterFilter.HasValue)
                    query = query.Where(c => c.DateUpdated >= dateUpdatedAfterFilter.Value.Date);

                if (!string.IsNullOrEmpty(friendlyNameFilter))
                    query = query.Where(c => c.FriendlyName.Equals(friendlyNameFilter));
                if (statusFilter != null && statusFilter.Length > 0)
                    query = query.Where(c => statusFilter.Contains(c.Status));

                query = query.OrderBy(c => c.DateCreated);

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
                var conferencesList = new PageList<Conference>(items, hasMore);
                return Task.FromResult(conferencesList);
            }
        }

        public Task UpdateConference(Conference conference)
        {
            if (string.IsNullOrEmpty(conference.Sid))
                throw new ArgumentException("No conference sid provided");

            lock (ConferencesLock)
            {
                var existing = Conferences.SingleOrDefault(c => c.Sid.Equals(conference.Sid, StringComparison.InvariantCultureIgnoreCase));
                if (existing == null)
                    throw new InvalidOperationException($"Conference with sid {conference.Sid} does not exist");
                else if (!existing.FriendlyName.Equals(conference.FriendlyName))
                    throw new InvalidOperationException("Friendly name of conference must not change");
                else
                    Conferences.Remove(existing); Conferences.Add(conference);
            }

            return Task.CompletedTask;
        }

        public Task Clear()
        {
            lock (ConferencesLock)
            {
                Conferences.Clear();
            }
            return Task.CompletedTask;
        }
    }
}
