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
    public class ConferenceParticipantRepository : IConferenceParticipantRepository
    {
        private readonly List<ConferenceParticipant> Participants = new List<ConferenceParticipant>();
        private readonly object ParticipantsLock = new object();

        public Task CreateConferenceParticipant(ConferenceParticipant conferenceParticipant)
        {
            if (string.IsNullOrEmpty(conferenceParticipant.ConferenceSid))
                throw new ArgumentException("No conference sid provided");
            if (string.IsNullOrEmpty(conferenceParticipant.CallSid))
                throw new ArgumentException("No call sid provided");
            lock (ParticipantsLock)
            {
                if (Participants.Any(p => p.ConferenceSid == conferenceParticipant.ConferenceSid && p.CallSid == conferenceParticipant.CallSid))
                    throw new InvalidOperationException($"Conference participant with conference sid {conferenceParticipant.ConferenceSid} and call sid {conferenceParticipant.CallSid} does already exist");
                Participants.Add(conferenceParticipant);
            }
            return Task.CompletedTask;
        }

        public Task<ConferenceParticipant> GetConferenceParticipant(string conferenceSid, string callSid)
        {
            if (string.IsNullOrEmpty(conferenceSid))
                throw new ArgumentException("No conferenceSid provided", nameof(conferenceSid));
            if (string.IsNullOrEmpty(callSid))
                throw new ArgumentException("No callSid provided", nameof(callSid));
            lock (ParticipantsLock)
            {
                var participant = Participants.SingleOrDefault(p => p.ConferenceSid == conferenceSid && p.CallSid == callSid);
                if (participant == null)
                    throw new NotFoundException($"{conferenceSid},{callSid}");
                return Task.FromResult(participant);
            }
        }

        public Task<ConferenceParticipantsPage> GetConferenceParticipants(Uri url)
        {
            var path = url.GetComponents(UriComponents.Path, UriFormat.SafeUnescaped);
            var pathMatch = Regex.Match(path, "^\\d{4}-\\d{2}-\\d{2}\\/Accounts\\/AC\\w{32}\\/Conferences\\/(?<confId>CF\\w{32})\\/Participants\\.json$");
            if (!pathMatch.Success)
                throw new ArgumentException("Url does not match regex", nameof(url));
            var conferenceSid = pathMatch.Groups["confId"].Value;

            var queryParams = HttpUtility.ParseQueryString(url.Query);

            var mutedFilter = queryParams.GetBool("Muted");
            var holdFilter = queryParams.GetBool("Hold");
            var page = queryParams.GetPage();
            var pageSize = queryParams.GetPageSize();
            var pageToken = queryParams.GetPageToken();

            lock (ParticipantsLock)
            {
                IEnumerable<ConferenceParticipant> query = Participants;

                query = query.Where(p => p.ConferenceSid == conferenceSid);

                if (mutedFilter.HasValue)
                    query = query.Where(p => p.Muted == mutedFilter.Value);
                if (holdFilter.HasValue)
                    query = query.Where(p => p.Hold == holdFilter.Value);

                query = query.OrderBy(p => p.DateCreated);

                if (page == 0 && string.IsNullOrEmpty(pageToken))
                    query = query.Take(pageSize);
                else if (page == 0 && !string.IsNullOrEmpty(pageToken))
                    query = query.TakeWhile(c => c.CallSid != pageToken).TakeLast(pageSize);
                else if (page > 0 && !string.IsNullOrEmpty(pageToken))
                    query = query.SkipWhile(c => c.CallSid != pageToken).Skip(1).Take(pageSize);
                else
                    throw new Exception($"Invalid paging with page={page}, pageSize={pageSize}, pageToken={pageToken}");

                var items = query.ToList();
                var hasMore = query.Take(1) != null;
                var callsPage = new ConferenceParticipantsPage(items, hasMore, url);
                return Task.FromResult(callsPage);
            }
        }

        public Task UpdateConferenceParticipant(ConferenceParticipant conferenceParticipant)
        {
            if (string.IsNullOrEmpty(conferenceParticipant.ConferenceSid))
                throw new ArgumentException("No conference sid provided");
            if (string.IsNullOrEmpty(conferenceParticipant.CallSid))
                throw new ArgumentException("No call sid provided");

            lock (ParticipantsLock)
            {
                var existing = Participants.SingleOrDefault(p => p.ConferenceSid == conferenceParticipant.ConferenceSid && p.CallSid == conferenceParticipant.CallSid);
                if (existing == null)
                    throw new NotFoundException($"{conferenceParticipant.ConferenceSid},{conferenceParticipant.CallSid}");
                else
                    Participants.Remove(existing); Participants.Add(conferenceParticipant);
            }

            return Task.CompletedTask;
        }

        public Task DeleteConferenceParticipant(string conferenceSid, string callSid)
        {
            if (string.IsNullOrEmpty(conferenceSid))
                throw new ArgumentException("No conferenceSid provided", nameof(conferenceSid));
            if (string.IsNullOrEmpty(callSid))
                throw new ArgumentException("No callSid provided", nameof(callSid));
            lock (ParticipantsLock)
            {
                var index = Participants.FindIndex(p => p.ConferenceSid == conferenceSid && p.CallSid == callSid);
                if (index >= 0)
                    throw new NotFoundException(callSid);
                else
                    Participants.RemoveAt(index);
            }
            return Task.CompletedTask;
        }
    }
}
