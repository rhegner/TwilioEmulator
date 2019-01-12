using System.Threading.Tasks;
using TwilioLogic.Models;
using TwilioLogic.TwilioModels;

namespace TwilioLogic.RepositoryInterfaces
{
    public interface IConferenceParticipantRepository
    {
        Task CreateConferenceParticipant(ConferenceParticipant conferenceParticipant);

        Task<ConferenceParticipant> GetConferenceParticipant(string conferenceSid, string callSid);

        Task<PageList<ConferenceParticipant>> GetConferenceParticipants(string conferenceSidFilter, bool? mutedFilter, bool? holdFilter,
            int page, int pageSize, string pageToken);

        Task UpdateConferenceParticipant(ConferenceParticipant call);

        Task DeleteConferenceParticipant(string conferenceSid, string callSid);

        Task Clear();
    }
}
