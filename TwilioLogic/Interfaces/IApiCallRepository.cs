using System.Collections.Generic;
using System.Threading.Tasks;
using TwilioLogic.Models;

namespace TwilioLogic.Interfaces
{
    public interface IApiCallRepository
    {
        Task CreateApiCall(ApiCall apiCall);
        Task<List<ApiCall>> GetApiCallsForResource(string sid);
    }
}
