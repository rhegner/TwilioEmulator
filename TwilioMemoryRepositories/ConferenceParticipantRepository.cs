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

        public Task<PageList<ConferenceParticipant>> GetConferenceParticipants(string conferenceSidFilter, bool? mutedFilter, bool? holdFilter,
            int page, int pageSize, string pageToken)
        {
            lock (ParticipantsLock)
            {
                IEnumerable<ConferenceParticipant> query = Participants;

                if (!string.IsNullOrEmpty(conferenceSidFilter))
                    query = query.Where(p => p.ConferenceSid == conferenceSidFilter);
                if (mutedFilter.HasValue)
                    query = query.Where(p => p.Muted == mutedFilter.Value);
                if (holdFilter.HasValue)
                    query = query.Where(p => p.Hold == holdFilter.Value);

                query = query.OrderByDescending(p => p.DateCreated);

                List<ConferenceParticipant> items = null;
                if (page == 0 && string.IsNullOrEmpty(pageToken))
                    items = query.Take(pageSize).ToList();
                else if (page == 0 && !string.IsNullOrEmpty(pageToken))
                    items = query.TakeWhile(c => c.CallSid != pageToken).TakeLast(pageSize).ToList();
                else if (page > 0 && !string.IsNullOrEmpty(pageToken))
                    items = query.SkipWhile(c => c.CallSid != pageToken).Skip(1).Take(pageSize).ToList();
                else
                    throw new Exception($"Invalid paging with page={page}, pageSize={pageSize}, pageToken={pageToken}");

                var hasMore = items.Count > 0 && items.Last() != query.Last();
                var participantsList = new PageList<ConferenceParticipant>(items, hasMore);
                return Task.FromResult(participantsList);
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

        public Task Clear()
        {
            lock (ParticipantsLock)
            {
                Participants.Clear();
            }
            return Task.CompletedTask;
        }
    }
}
