using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using TwilioLogic.Models;

namespace TwilioEmulator.Hubs
{
    public class ApiCallsHub : Hub<IApiCallsClient>
    { }

    public interface IApiCallsClient
    {
        Task NewApiCall(ApiCall apiCall);
    }
}
