using System.Collections.Generic;
using System.Threading.Tasks;
using TwilioLogic.Models;

namespace TwilioLogic.Interfaces
{
    public interface ICallResouceRepository
    {

        Task Create(CallResource call);

        Task<CallResource> Get(string sid);

        Task<Page<CallResource>> Get(ICollection<string> directionFilter = null, ICollection<string> statusFilter = null, long page = 1, long pageSize = long.MaxValue);

        Task Update(CallResource call);

    }
}
