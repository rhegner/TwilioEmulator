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

        private readonly ConcurrentDictionary<Guid, ActivityLog> Logs = new ConcurrentDictionary<Guid, ActivityLog>();

        public Task CreateActivityLog(ActivityLog activityLog)
        {
            if (!Logs.TryAdd(activityLog.ActivityLogId, activityLog))
                throw new InvalidOperationException($"{activityLog.ActivityLogId} already exists");
            return Task.CompletedTask;
        }

        public Task<List<ActivityLog>> GetActivityLogsForResource(string sid)
        {
            return Task.FromResult(Logs.Values.Where(al => al.Sid == sid).OrderBy(ac => ac.Timestamp).ToList());
        }
    }
}
