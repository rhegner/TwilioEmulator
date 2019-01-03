using System.Collections.Generic;
using System.Threading.Tasks;
using TwilioLogic.Models;

namespace TwilioLogic.Interfaces
{
    public interface IActivityLogRepository
    {
        Task CreateActivityLog(ActivityLog activityLog);
        Task<List<ActivityLog>> GetActivityLogsForResource(string sid);
    }
}
