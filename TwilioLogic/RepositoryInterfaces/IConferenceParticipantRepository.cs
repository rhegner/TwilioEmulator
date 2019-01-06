using System;
using System.Threading.Tasks;
using TwilioLogic.TwilioModels;

namespace TwilioLogic.RepositoryInterfaces
{
    public interface IConferenceParticipantRepository
    {
        Task CreateConferenceParticipant(ConferenceParticipant conferenceParticipant);

        Task<ConferenceParticipant> GetConferenceParticipant(string conferenceSid, string callSid);

        Task<ConferenceParticipantsPage> GetConferenceParticipants(Uri url);

        Task UpdateConferenceParticipant(ConferenceParticipant call);

        Task DeleteConferenceParticipant(string conferenceSid, string callSid);
    }
}
