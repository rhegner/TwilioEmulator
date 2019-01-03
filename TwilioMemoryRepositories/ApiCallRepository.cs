using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwilioLogic.Interfaces;
using TwilioLogic.Models;

namespace TwilioMemoryRepositories
{
    public class ApiCallRepository : IApiCallRepository
    {

        private readonly ConcurrentDictionary<Guid, ApiCall> Calls = new ConcurrentDictionary<Guid, ApiCall>();

        public Task CreateApiCall(ApiCall apiCall)
        {
            if (!Calls.TryAdd(apiCall.ApiCallId, apiCall))
                throw new InvalidOperationException($"{apiCall.ApiCallId} already exists");
            return Task.CompletedTask;
        }

        public Task<List<ApiCall>> GetApiCallsForResource(string sid)
        {
            return Task.FromResult(Calls.Values.Where(ac => ac.Sid == sid).OrderBy(ac => ac.RequestTimestamp).ToList());
        }
    }
}
