using System;
using System.Threading.Tasks;
using TwilioLogic.Models;
using TwilioLogic.TwilioModels;

namespace TwilioLogic.RepositoryInterfaces
{
    public interface IConferenceRepository
    {

        Task<Conference> GetOrCreateConference(string friendlyName, Func<string, Conference> factory);

        Task<Conference> GetConference(string sid);

        Task<PageList<Conference>> GetConferences(DateTime? dateCreatedFilter, DateTime? dateCreatedBeforeFilter, DateTime? dateCreatedAfterFilter,
            DateTime? dateUpdatedFilter, DateTime? dateUpdatedBeforeFilter, DateTime? dateUpdatedAfterFilter,
            string friendlyNameFilter, string[] statusFilter,
            int page, int pageSize, string pageToken);
        
        Task UpdateConference(Conference conference);

        Task Clear();
    }
}
