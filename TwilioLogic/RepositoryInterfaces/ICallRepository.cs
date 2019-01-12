using System;
using System.Threading.Tasks;
using TwilioLogic.Models;
using TwilioLogic.TwilioModels;

namespace TwilioLogic.RepositoryInterfaces
{
    public interface ICallRepository
    {
        Task CreateCall(Call call);

        Task<Call> GetCall(string callSid);

        Task<PageList<Call>> GetCalls(string toFilter, string fromFilter, string parentCallSidFilter, string[] statusFilter,
            DateTime? startTimeFilter, DateTime? startTimeBeforeFilter, DateTime? startTimeAfterFilter,
            DateTime? endTimeFilter, DateTime? endTimeBeforeFilter, DateTime? endTimeAfterFilter,
            int page, int pageSize, string pageToken);

        Task UpdateCall(Call call);

        Task DeleteCall(string callSid);
    }
}
