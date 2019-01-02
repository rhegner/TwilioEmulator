using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using TwilioLogic.Models;

namespace TwilioEmulator.Hubs
{
    public class CallResourcesHub : Hub<ICallResourcesClient>
    { }

    public interface ICallResourcesClient
    {
        Task CallResourceUpdate(CallResource callResource, bool isNew);
    }
}
