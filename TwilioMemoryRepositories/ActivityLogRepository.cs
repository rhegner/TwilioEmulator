using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwilioLogic.Models;
using TwilioLogic.RepositoryInterfaces;

namespace TwilioMemoryRepositories
{
    public class ActivityLogRepository : IActivityLogRepository
    {

        private readonly ConcurrentDictionary<string, ActivityLog> Logs = new ConcurrentDictionary<string, ActivityLog>();

        public Task CreateActivityLog(ActivityLog activityLog)
        {
            if (!Logs.TryAdd(activityLog.Sid, activityLog))
                throw new InvalidOperationException($"{activityLog.Sid} already exists");
            return Task.CompletedTask;
        }

        public Task<List<ActivityLog>> GetActivityLogsForResource(string sid)
        {
            return Task.FromResult(Logs.Values.Where(al => al.Sid == sid).OrderBy(ac => ac.Timestamp).ToList());
        }

        public Task Clear()
        {
            Logs.Clear();
            return Task.CompletedTask;
        }
    }
}
