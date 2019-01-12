using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using TwilioLogic.EventModels;
using TwilioLogic.Models;
using TwilioLogic.RepositoryInterfaces;
using TwilioLogic.TwilioModels;
using TwilioLogic.Utils;

namespace TwilioLogic
{
    public class TwilioEngine
    {

        private readonly IAccountRepository AccountRepository;
        private readonly ICallRepository CallRepository;
        private readonly IConferenceRepository ConferenceRepository;
        private readonly IConferenceParticipantRepository ConferenceParticipantRepository;
        private readonly IAlertRepository AlertRepository;
        private readonly IActivityLogRepository ActivityLogRepository;
        private readonly ILogger<TwilioEngine> Logger;

        public event EventHandler<ResourceCudOperationEventArgs<Call>> CallCudOperation;
        public event EventHandler<ResourceCudOperationEventArgs<Conference>> ConferenceCudOperation;
        public event EventHandler<ResourceCudOperationEventArgs<ConferenceParticipant>> ConferenceParticipantCudOperation;
        public event EventHandler<ResourceCudOperationEventArgs<Alert>> AlertCudOperation;

        public event EventHandler<ResourceCudOperationEventArgs<ActivityLog>> ActivityLogCudOperation;
        
        public TwilioEngine(IAccountRepository accountRepository, ICallRepository callRepository, IConferenceRepository conferenceRepository, IConferenceParticipantRepository conferenceParticipantRepository,
            IAlertRepository alertRepository, IActivityLogRepository activityLogRepository, ILogger<TwilioEngine> logger)
        {
            AccountRepository = accountRepository;
            CallRepository = callRepository;
            ConferenceRepository = conferenceRepository;
            ConferenceParticipantRepository = conferenceParticipantRepository;
            AlertRepository = alertRepository;
            ActivityLogRepository = activityLogRepository;
            Logger = logger;
        }

        public async Task ClearDatabase()
        {
            await AccountRepository.Clear();

            await CallRepository.Clear();
            CallCudOperation?.Invoke(this, new ResourceCudOperationEventArgs<Call>(null, ResourceCudOperation.Reset));

            await ConferenceRepository.Clear();
            ConferenceCudOperation?.Invoke(this, new ResourceCudOperationEventArgs<Conference>(null, ResourceCudOperation.Reset));

            await ConferenceParticipantRepository.Clear();
            ConferenceParticipantCudOperation?.Invoke(this, new ResourceCudOperationEventArgs<ConferenceParticipant>(null, ResourceCudOperation.Reset));

            await AlertRepository.Clear();
            AlertCudOperation?.Invoke(this, new ResourceCudOperationEventArgs<Alert>(null, ResourceCudOperation.Reset));

            await ActivityLogRepository.Clear();
            ActivityLogCudOperation?.Invoke(this, new ResourceCudOperationEventArgs<ActivityLog>(null, ResourceCudOperation.Reset));
        }

        #region public call interface

        public async Task<Call> CreateIncomingCall(string from, string to, string url, string method)
        {
            var call = new Call()
            {
                AccountSid = AccountRepository.GetAccountSid(),
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                Direction = "inbound",
                From = from,
                PhoneNumberSid = await AccountRepository.GetPhoneNumberSid(to),
                Sid = TwilioUtils.CreateSid("CA"),
                Status = "ringing",
                To = to
            };
            await CallRepository.CreateCall(call);
            CallCudOperation?.Invoke(this, new ResourceCudOperationEventArgs<Call>(call, ResourceCudOperation.Create));
            CallHandler(url, method, call.Sid);
            return call;
        }

        public async Task<Call> CreateCall(string accountSid, string apiVersion, string from, string method, string to, string url)
        {
            var call = new Call()
            {
                AccountSid = accountSid,
                ApiVersion = apiVersion,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                Direction = "outbound-api",
                From = from,
                PhoneNumberSid = await AccountRepository.GetPhoneNumberSid(from),
                Sid = TwilioUtils.CreateSid("CA"),
                Status = "queued",
                To = to
            };
            await CallRepository.CreateCall(call);
            CallCudOperation?.Invoke(this, new ResourceCudOperationEventArgs<Call>(call, ResourceCudOperation.Create));
            CallHandler(url, method, call.Sid);
            return call;
        }

        public Task<Call> GetCall(string callSid)
            => CallRepository.GetCall(callSid);

        public Task<PageList<Call>> GetCalls(string toFilter, string fromFilter, string parentCallSidFilter, string[] statusFilter,
            DateTime? startTimeFilter, DateTime? startTimeBeforeFilter, DateTime? startTimeAfterFilter,
            DateTime? endTimeFilter, DateTime? endTimeBeforeFilter, DateTime? endTimeAfterFilter,
            int page, int pageSize, string pageToken)
            => CallRepository.GetCalls(toFilter, fromFilter, parentCallSidFilter, statusFilter,
                startTimeFilter, startTimeBeforeFilter, startTimeAfterFilter,
                endTimeFilter, endTimeBeforeFilter, endTimeAfterFilter,
                page, pageSize, pageToken);

        public async Task<Call> UpdateCall(string callSid)
        {
            var call = await CallRepository.GetCall(callSid);
            // TODO: update
            CallCudOperation?.Invoke(this, new ResourceCudOperationEventArgs<Call>(call, ResourceCudOperation.Update));
            return call;
        }

        public async Task DeleteCall(string callSid)
        {
            var call = await CallRepository.GetCall(callSid);
            if (call.Status == "in-progress")
                throw new InvalidOperationException("Can't delete a call which is in progress");
            await CallRepository.DeleteCall(call.Sid);
            CallCudOperation?.Invoke(this, new ResourceCudOperationEventArgs<Call>(call, ResourceCudOperation.Delete));
        }

        #endregion

        #region public conference interface

        public Task<Conference> GetConference(string conferenceSid)
            => ConferenceRepository.GetConference(conferenceSid);

        public Task<PageList<Conference>> GetConferences(DateTime? dateCreatedFilter, DateTime? dateCreatedBeforeFilter, DateTime? dateCreatedAfterFilter,
            DateTime? dateUpdatedFilter, DateTime? dateUpdatedBeforeFilter, DateTime? dateUpdatedAfterFilter,
            string friendlyNameFilter, string[] statusFilter,
            int page, int pageSize, string pageToken)
            => ConferenceRepository.GetConferences(dateCreatedFilter, dateCreatedBeforeFilter, dateCreatedAfterFilter,
                dateUpdatedFilter, dateUpdatedBeforeFilter, dateUpdatedAfterFilter,
                friendlyNameFilter, statusFilter,
                page, pageSize, pageToken);

        public async Task<Conference> UpdateConference(string conferenceSid)
        {
            var conference = await ConferenceRepository.GetConference(conferenceSid);
            // TODO: update
            ConferenceCudOperation?.Invoke(this, new ResourceCudOperationEventArgs<Conference>(conference, ResourceCudOperation.Update));
            return conference;
        }

        public Task<ConferenceParticipant> GetConferenceParticipant(string conferenceSid, string callSid)
            => ConferenceParticipantRepository.GetConferenceParticipant(conferenceSid, callSid);

        public Task<PageList<ConferenceParticipant>> GetConferenceParticipants(string conferenceSidFilter, bool? mutedFilter, bool? holdFilter,
            int page, int pageSize, string pageToken)
            => ConferenceParticipantRepository.GetConferenceParticipants(conferenceSidFilter, mutedFilter, holdFilter,
                page, pageSize, pageToken);

        public async Task<ConferenceParticipant> UpdateConferenceParticipant(string conferenceSid, string callSid)
        {
            var participant = await ConferenceParticipantRepository.GetConferenceParticipant(conferenceSid, callSid);
            // TODO: apply logic
            ConferenceParticipantCudOperation?.Invoke(this, new ResourceCudOperationEventArgs<ConferenceParticipant>(participant, ResourceCudOperation.Update));
            return participant;
        }

        public async Task DeleteConferenceParticipant(string conferenceSid, string callSid)
        {
            var participant = await ConferenceParticipantRepository.GetConferenceParticipant(conferenceSid, callSid);
            // TODO: apply logic
            await ConferenceParticipantRepository.DeleteConferenceParticipant(conferenceSid, callSid);
            ConferenceParticipantCudOperation?.Invoke(this, new ResourceCudOperationEventArgs<ConferenceParticipant>(participant, ResourceCudOperation.Delete));
        }

        #endregion

        #region public alert interface

        public async Task<Notification> GetAlert(string alertSid)
            => new Notification(await AlertRepository.GetAlert(alertSid));

        public Task<PageList<Alert>> GetAlerts(string resourceSidFilter, string logLevelFilter,
            DateTime? messageDateFilter, DateTime? messageDateBeforeFilter, DateTime? messageDateAfterFilter,
            int page, int pageSize, string pageToken)
            => AlertRepository.GetAlerts(resourceSidFilter, logLevelFilter,
                messageDateFilter, messageDateBeforeFilter, messageDateAfterFilter,
                page, pageSize, pageToken);

        public async Task DeleteAlert(string alertSid)
        {
            var alert = await AlertRepository.GetAlert(alertSid);
            await AlertRepository.DeleteAlert(alertSid);
            AlertCudOperation?.Invoke(this, new ResourceCudOperationEventArgs<Alert>(alert, ResourceCudOperation.Delete));
        }

        #endregion

        #region public activity log interface

        public Task<List<ActivityLog>> GetActivityLogs(string sid)
            => ActivityLogRepository.GetActivityLogsForResource(sid);

        #endregion

        #region private implementation

        private async void CallHandler(string url, string httpMethod, string callSid)
        {
            if (string.IsNullOrEmpty(url))
                return;
            if (string.IsNullOrEmpty(httpMethod))
                httpMethod = "POST";

            try
            {
                // TODO:
                /*
                var apiCall = new ApiCall()
                {
                    ApiCallId = Guid.NewGuid(),
                    Sid = callSid,
                    Direction = ApiCallDirection.FromEmulator,
                    Type = ApiCallType.IncomingCallCallback,
                    HttpMethod = httpMethod.ToString()
                };
                */

                var call = await CallRepository.GetCall(callSid);
                var callParams = new NameValueCollection();
                callParams.Add("CallSid", call.Sid);
                callParams.Add("AccountSid", call.AccountSid);
                callParams.Add("From", call.From);
                callParams.Add("To", call.To);
                callParams.Add("CallStatus", call.Status);
                callParams.Add("ApiVersion", call.ApiVersion);
                callParams.Add("Direction", call.Direction);
                callParams.Add("ForwardedFrom", call.ForwardedFrom);
                callParams.Add("CallerName", call.CallerName);
                callParams.Add("ParentCallSid", call.ParentCallSid);

                HttpRequestMessage request;
                if (httpMethod.Equals("GET", StringComparison.InvariantCultureIgnoreCase))
                {
                    var uriBuilder = new UriBuilder(url);
                    var queryParams = HttpUtility.ParseQueryString(uriBuilder.Query);
                    queryParams.Add(callParams);
                    uriBuilder.Query = queryParams.ToString();
                    request = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);
                }
                if (httpMethod.Equals("POST", StringComparison.InvariantCultureIgnoreCase))
                {
                    request = new HttpRequestMessage(HttpMethod.Post, url);
                    request.Content = new FormUrlEncodedContent(callParams.AllKeys.Select(k => new KeyValuePair<string, string>(k, callParams[k])));

                    // TODO:
                    //apiCall.RequestContentType = request.Content.Headers.ContentType.ToString();
                    //apiCall.RequestContent = await request.Content.ReadAsStringAsync();
                }
                else
                {
                    throw new NotSupportedException($"{httpMethod} is not supported");
                }

                // TODO:
                //apiCall.Url = url;

                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        // TODO:
                        // apiCall.RequestTimestamp = DateTime.UtcNow;
                        using (var response = await httpClient.SendAsync(request))
                        {
                            // TODO:
                            /*
                            apiCall.ResponseTimestamp = DateTime.UtcNow;
                            if (response.Content != null)
                            {
                                apiCall.ResponseContentType = response.Content.Headers.ContentType.ToString();
                                apiCall.ResponseContent = await response.Content.ReadAsStringAsync();
                            }
                            apiCall.ResponseStatusCode = (int)response.StatusCode;
                            */
                        }
                    }
                }
                catch (Exception ex)
                {
                    // TODO:
                    // apiCall.ResponseStatusCode = -1;
                    Logger.LogError(ex, "Error sending callback request");
                }

                // TODO:
                /*
                await ApiCallRepository.CreateApiCall(apiCall);
                NewApiCall?.Invoke(this, new NewApiCallEventArgs(apiCall));
                */

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error invoking callback");
            }
        }

        #endregion

    }
}
