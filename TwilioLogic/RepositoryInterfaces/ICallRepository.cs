using System;
using System.Threading.Tasks;
using TwilioLogic.TwilioModels;

namespace TwilioLogic.RepositoryInterfaces
{
    public interface ICallRepository
    {
        Task CreateCall(Call call);

        Task<Call> GetCall(string callSid);

        Task<CallsPage> GetCalls(Uri url);

        Task UpdateCall(Call call);

        Task DeleteCall(string callSid);
    }
}
