using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwilioLogic.Interfaces;
using TwilioLogic.Models;

namespace TwilioMemoryRepositories
{
    public class ConferenceResourceRepository : IConferenceResourceRepository
    {

        private readonly List<ConferenceResource> Conferences = new List<ConferenceResource>();
        private readonly object ConferencesLock = new object();

        public Task<ConferenceResource> GetOrCreate(string friendlyName, Func<string, ConferenceResource> factory)
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

        public Task<ConferenceResource> Get(string sid)
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

        public Task<Page<ConferenceResource>> Get(ICollection<string> statusFilter = null, int page = 1, int pageSize = int.MaxValue)
        {
            lock (ConferencesLock)
            {
                IEnumerable<ConferenceResource> query = Conferences;
                if (statusFilter != null && statusFilter.Count > 0)
                    query = query.Where(c => statusFilter.Contains(c.Status));
                query = query.OrderByDescending(c => c.DateCreated);
                query = query.Skip((page - 1) * pageSize).Take(pageSize);
                return Task.FromResult(new Page<ConferenceResource>(page, Conferences.Count, pageSize, query.ToList()));
            }
        }

        public Task Update(ConferenceResource conference)
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

    }
}
