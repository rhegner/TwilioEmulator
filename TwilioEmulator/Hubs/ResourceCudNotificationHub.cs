using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using TwilioLogic.EventModels;
using TwilioLogic.Interfaces;

namespace TwilioEmulator.Hubs
{
    public class ResourceCudNotificationHub<T> : Hub<IResourceCudNotificationClient<T>>
        where T: IResource
    { }

    public interface IResourceCudNotificationClient<T>
        where T: IResource
    {
        Task ResourceCudOperation(T resource, ResourceCudOperation operation);
    }
}
