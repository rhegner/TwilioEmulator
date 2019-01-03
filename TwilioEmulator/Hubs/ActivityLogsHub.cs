using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using TwilioLogic.Models;

namespace TwilioEmulator.Hubs
{
    public class ActivityLogsHub : Hub<IActivityLogsClient>
    { }

    public interface IActivityLogsClient
    {
        Task NewActivityLog(ActivityLog activityLog);
    }
}
