using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwilioLogic.Interfaces;
using TwilioLogic.Models;

namespace TwilioMemoryRepositories
{
    public class CallResourceRepository : ICallResouceRepository
    {

        private readonly List<CallResource> Calls = new List<CallResource>();
        private readonly object CallsLock = new object();

        public Task Create(CallResource call)
        {
            if (string.IsNullOrEmpty(call.Sid))
                throw new ArgumentException("No call sid provided");
            lock (CallsLock)
            {
                if (Calls.Any(c => c.Sid.Equals(call.Sid, StringComparison.InvariantCultureIgnoreCase)))
                    throw new InvalidOperationException($"Call with sid {call.Sid} does already exist");
                Calls.Add(call);
            }
            return Task.CompletedTask;
        }

        public Task<CallResource> Get(string sid)
        {
            if (string.IsNullOrEmpty(sid))
                throw new ArgumentException("No call sid provided", nameof(sid));
            lock (CallsLock)
            {
                var call = Calls.SingleOrDefault(c => c.Sid == sid);
                if (call == null)
                    throw new InvalidOperationException($"Call with sid {sid} not found");
                return Task.FromResult(call);
            }
        }

        public Task<Page<CallResource>> Get(ICollection<string> directionFilter = null, ICollection<string> statusFilter = null, long page = 1, long pageSize = long.MaxValue)
        {
            lock (CallsLock)
            {
                IEnumerable<CallResource> query = Calls;
                if (directionFilter != null && directionFilter.Count > 0)
                    query = query.Where(c => directionFilter.Contains(c.Direction));
                if (statusFilter != null && statusFilter.Count > 0)
                    query = query.Where(c => statusFilter.Contains(c.Status));
                query = query.OrderByDescending(c => c.DateCreated);
                return Task.FromResult(new Page<CallResource>(page, Calls.Count, pageSize, query.ToList()));
            }
        }

        public Task Update(CallResource call)
        {
            if (string.IsNullOrEmpty(call.Sid))
                throw new ArgumentException("No call sid provided");

            lock (CallsLock)
            {
                var existing = Calls.SingleOrDefault(c => c.Sid.Equals(call.Sid, StringComparison.InvariantCultureIgnoreCase));
                if (existing == null)
                    throw new InvalidOperationException($"Call with sid {call.Sid} does not exist");
                else
                    Calls.Remove(existing); Calls.Add(call);
            }

            return Task.CompletedTask;
        }
    }
}
