using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using TwilioLogic.EventModels;
using TwilioLogic.Interfaces;

namespace TwilioEmulator.Hubs
{
    public class ResourceCudNotificationHub<T> : Hub<IResourceCudNotificationClient<T>>
        where T: IResource
    {
        public override async Task OnConnectedAsync()
        {
            if (Context.GetHttpContext().Request.Query.ContainsKey("topLevelSidFilter"))
                await Groups.AddToGroupAsync(Context.ConnectionId, Context.GetHttpContext().Request.Query["topLevelSidFilter"].ToString());
            else
                await Groups.AddToGroupAsync(Context.ConnectionId, "*");
            await base.OnConnectedAsync();
        }
    }

    public interface IResourceCudNotificationClient<T>
        where T: IResource
    {
        Task ResourceCudOperation(T resource, ResourceCudOperation operation);
    }
}
