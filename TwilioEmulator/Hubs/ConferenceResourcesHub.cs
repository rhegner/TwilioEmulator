using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using TwilioLogic.Models;

namespace TwilioEmulator.Hubs
{
    public class ConferenceResourcesHub : Hub<IConferenceResourcesClient>
    { }

    public interface IConferenceResourcesClient
    {
        Task ConferenceResourceUpdate(ConferenceResource conferenceResource);
    }
}
