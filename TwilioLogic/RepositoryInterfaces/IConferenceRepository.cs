using System;
using System.Threading.Tasks;
using TwilioLogic.TwilioModels;

namespace TwilioLogic.RepositoryInterfaces
{
    public interface IConferenceRepository
    {

        Task<Conference> GetOrCreateConference(string friendlyName, Func<string, Conference> factory);

        Task<Conference> GetConference(string sid);

        Task<ConferencesPage> GetConferences(Uri url);
        
        Task UpdateConference(Conference conference);
    }
}
