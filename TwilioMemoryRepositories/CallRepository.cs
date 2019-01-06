using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TwilioLogic.Exceptions;
using TwilioLogic.RepositoryInterfaces;
using TwilioLogic.TwilioModels;

namespace TwilioMemoryRepositories
{
    public class CallRepository : ICallRepository
    {
        private readonly List<Call> Calls = new List<Call>();
        private readonly object CallsLock = new object();

        public Task CreateCall(Call call)
        {
            if (string.IsNullOrEmpty(call.Sid))
                throw new ArgumentException("No call sid provided");
            lock (CallsLock)
            {
                if (Calls.Any(c => c.Sid.Equals(call.Sid, StringComparison.InvariantCultureIgnoreCase)))
                    throw new InvalidOperationException($"Call with sid {call.Sid} does already exist");
                Calls.Add(call);
            }
            return Task.CompletedTask;
        }

        public Task<Call> GetCall(string callSid)
        {
            if (string.IsNullOrEmpty(callSid))
                throw new ArgumentException("No callSid provided", nameof(callSid));
            lock (CallsLock)
            {
                var call = Calls.SingleOrDefault(c => c.Sid == callSid);
                if (call == null)
                    throw new NotFoundException(callSid);
                return Task.FromResult(call);
            }
        }

        public Task<CallsPage> GetCalls(Uri url)
        {
            var queryParams = HttpUtility.ParseQueryString(url.Query);

            var toFilter = queryParams.Get("To");
            var fromFilter = queryParams.Get("From");
            var parentCallSidFilter = queryParams.Get("ParentCallSid");
            var statusFilter = queryParams.GetStringArray("Status");
            var startTimeFilter = queryParams.GetDateTime("StartTime");
            var startTimeBeforeFilter = queryParams.GetDateTime("StartTime<");
            var startTimeAfterFilter = queryParams.GetDateTime("StartTime>");
            var endTimeFilter = queryParams.GetDateTime("EndTime");
            var endTimeBeforeFilter = queryParams.GetDateTime("EndTime<");
            var endTimeAfterFilter = queryParams.GetDateTime("EndTime>");
            var page = queryParams.GetPage();
            var pageSize = queryParams.GetPageSize();
            var pageToken = queryParams.GetPageToken();

            lock (CallsLock)
            {
                IEnumerable<Call> query = Calls;

                if (!string.IsNullOrEmpty(toFilter))
                    query = query.Where(c => c.To.Equals(toFilter));
                if (!string.IsNullOrEmpty(fromFilter))
                    query = query.Where(c => c.From.Equals(fromFilter));
                if (!string.IsNullOrEmpty(parentCallSidFilter))
                    query = query.Where(c => c.ParentCallSid.Equals(parentCallSidFilter));
                if (statusFilter != null && statusFilter.Length > 0)
                    query = query.Where(c => statusFilter.Contains(c.Status));

                if (startTimeFilter.HasValue)
                    query = query.Where(c => c.StartTime.HasValue && c.StartTime.Value >= startTimeFilter.Value.Date && c.StartTime.Value < startTimeFilter.Value.AddDays(1).Date);
                if (startTimeBeforeFilter.HasValue)
                    query = query.Where(c => c.StartTime.HasValue && c.StartTime.Value < startTimeBeforeFilter.Value.Date);
                if (startTimeAfterFilter.HasValue)
                    query = query.Where(c => c.StartTime.HasValue && c.StartTime.Value >= startTimeAfterFilter.Value.Date);
                if (endTimeFilter.HasValue)
                    query = query.Where(c => c.EndTime.HasValue && c.EndTime.Value >= endTimeFilter.Value.Date && c.EndTime.Value < endTimeFilter.Value.AddDays(1).Date);
                if (endTimeBeforeFilter.HasValue)
                    query = query.Where(c => c.EndTime.HasValue && c.EndTime.Value < endTimeBeforeFilter.Value.Date);
                if (endTimeAfterFilter.HasValue)
                    query = query.Where(c => c.EndTime.HasValue && c.EndTime.Value >= endTimeAfterFilter.Value.Date);

                query = query.OrderBy(c => c.StartTime);

                if (page == 0 && string.IsNullOrEmpty(pageToken))
                    query = query.Take(pageSize);
                else if (page == 0 && !string.IsNullOrEmpty(pageToken))
                    query = query.TakeWhile(c => c.Sid != pageToken).TakeLast(pageSize);
                else if (page > 0 && !string.IsNullOrEmpty(pageToken))
                    query = query.SkipWhile(c => c.Sid != pageToken).Skip(1).Take(pageSize);
                else
                    throw new Exception($"Invalid paging with page={page}, pageSize={pageSize}, pageToken={pageToken}");

                var items = query.ToList();
                var hasMore = query.Take(1) != null;
                var callsPage = new CallsPage(items, hasMore, url);
                return Task.FromResult(callsPage);
            }
        }

        public Task UpdateCall(Call call)
        {
            if (string.IsNullOrEmpty(call.Sid))
                throw new ArgumentException("No call sid provided");

            lock (CallsLock)
            {
                var existing = Calls.SingleOrDefault(c => c.Sid.Equals(call.Sid, StringComparison.InvariantCultureIgnoreCase));
                if (existing == null)
                    throw new NotFoundException(call.Sid);
                else
                    Calls.Remove(existing); Calls.Add(call);
            }

            return Task.CompletedTask;
        }

        public Task DeleteCall(string callSid)
        {
            if (string.IsNullOrEmpty(callSid))
                throw new ArgumentException("No callSid provided", nameof(callSid));
            lock (CallsLock)
            {
                var index = Calls.FindIndex(c => c.Sid.Equals(callSid, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                    throw new NotFoundException(callSid);
                else
                    Calls.RemoveAt(index);
            }
            return Task.CompletedTask;
        }
    }
}
