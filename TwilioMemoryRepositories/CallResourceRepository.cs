using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwilioLogic.Interfaces;
using TwilioLogic.Models;

namespace TwilioMemoryRepositories
{
    public class CallResourceRepository : ICallResouceRepository
    {

        private readonly ConcurrentDictionary<string, CallResource> Calls = new ConcurrentDictionary<string, CallResource>();

        public Task Create(CallResource call)
        {
            if (string.IsNullOrEmpty(call.Sid))
                throw new ArgumentException("No call sid provided");
            if (!Calls.TryAdd(call.Sid, call))
                throw new InvalidOperationException($"{call.Sid} already exists");
            return Task.CompletedTask;
        }

        public Task<CallResource> Get(string sid)
        {
            if (string.IsNullOrEmpty(sid))
                throw new ArgumentException("No call sid provided");
            if (Calls.TryGetValue(sid, out var call))
                return Task.FromResult(call);
            else
                throw new InvalidOperationException($"{sid} not found.");
        }

        public Task<Page<CallResource>> Get(ICollection<string> directionFilter = null, ICollection<string> statusFilter = null, long page = 1, long pageSize = long.MaxValue)
        {
            var allCalls = Calls.Values;
            IEnumerable<CallResource> query = allCalls;
            if (directionFilter != null && directionFilter.Count > 0)
                query = query.Where(c => directionFilter.Contains(c.Direction));
            if (statusFilter != null && statusFilter.Count > 0)
                query = query.Where(c => statusFilter.Contains(c.Status));
            query = query.OrderByDescending(c => c.DateCreated);
            return Task.FromResult(new Page<CallResource>(page, allCalls.Count, pageSize, query.ToList()));
        }

        public Task Update(CallResource call)
        {
            if (string.IsNullOrEmpty(call.Sid))
                throw new ArgumentException("No call sid provided");

            // HACK: this separate check destroys atomicity of this operation.
            if (!Calls.ContainsKey(call.Sid))
                throw new ArgumentException($"{call.Sid} not found.");
            Calls[call.Sid] = call;

            return Task.CompletedTask;
        }
    }
}
