using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TwilioLogic.Models;

namespace TwilioLogic.Interfaces
{
    public interface IConferenceResourceRepository
    {

        Task<ConferenceResource> GetOrCreate(string friendlyName, Func<string, ConferenceResource> factory);

        Task<ConferenceResource> Get(string sid);

        Task<Page<ConferenceResource>> Get(ICollection<string> statusFilter = null, long page = 1, long pageSize = long.MaxValue);

        Task Update(ConferenceResource conference);
    }
}
