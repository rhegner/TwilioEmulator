using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Rest.Api.V2010.Account.Conference;
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
        private readonly IActivityLogRepository ActivityLogRepository;
        private readonly ILogger<TwilioEngine> Logger;

        public event EventHandler<ResourceCudOperationEventArgs<Call>> CallCudOperation;
        public event EventHandler<ResourceCudOperationEventArgs<Conference>> ConferenceCudOperation;
        public event EventHandler<ResourceCudOperationEventArgs<ConferenceParticipant>> ConferenceParticipantCudOperation;
        public event EventHandler<NewActivityLogEventArgs> NewActivityLog;

        public TwilioEngine(IAccountRepository accountRepository, ICallRepository callRepository, IConferenceRepository conferenceRepository, IConferenceParticipantRepository conferenceParticipantRepository,
            IActivityLogRepository activityLogRepository, ILogger<TwilioEngine> logger)
        {
            AccountRepository = accountRepository;
            CallRepository = callRepository;
            ConferenceRepository = conferenceRepository;
            ConferenceParticipantRepository = conferenceParticipantRepository;
            ActivityLogRepository = activityLogRepository;
            Logger = logger;
        }

        #region public call interface

        public async Task<Call> CreateIncomingCall(string from, string to, Uri url, HttpMethod httpMethod)
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
            CallHandler(url, httpMethod, call.Sid);
            return call;
        }

        public async Task<Call> CreateCall(CreateCallOptions options, string apiVersion)
        {
            var call = new Call()
            {
                AccountSid = options.PathAccountSid,
                ApiVersion = apiVersion,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                Direction = "outbound-api",
                From = options.From.ToString(),
                PhoneNumberSid = await AccountRepository.GetPhoneNumberSid(options.From.ToString()),
                Sid = TwilioUtils.CreateSid("CA"),
                Status = CallResource.StatusEnum.Queued.ToString(),
                To = options.To.ToString()
            };
            await CallRepository.CreateCall(call);
            CallCudOperation?.Invoke(this, new ResourceCudOperationEventArgs<Call>(call, ResourceCudOperation.Create));
            CallHandler(options.Url, new HttpMethod(options.Method.ToString()), call.Sid);
            return call;
        }

        public Task<Call> FetchCall(FetchCallOptions options)
            => CallRepository.GetCall(options.PathSid);

        public Task<CallsPage> GetCallsPage(Uri url)
            => CallRepository.GetCalls(url);

        // we need to pass in callSid separately, because we can't write it from the controller action
        public async Task<Call> UpdateCall(string callSid, UpdateCallOptions options)
        {
            var call = await CallRepository.GetCall(callSid);
            // TODO: update
            CallCudOperation?.Invoke(this, new ResourceCudOperationEventArgs<Call>(call, ResourceCudOperation.Update));
            return call;
        }

        public async Task DeleteCall(DeleteCallOptions options)
        {
            var call = await CallRepository.GetCall(options.PathSid);
            if (call.Status == CallResource.StatusEnum.InProgress.ToString())
                throw new InvalidOperationException("Can't delete a call which is in progress");
            await CallRepository.DeleteCall(call.Sid);
            CallCudOperation?.Invoke(this, new ResourceCudOperationEventArgs<Call>(call, ResourceCudOperation.Delete));
        }

        #endregion

        #region public conference interface

        public Task<Conference> FetchConference(FetchConferenceOptions options)
            => ConferenceRepository.GetConference(options.PathSid);

        public Task<ConferencesPage> GetConferencesPage(Uri url)
            => ConferenceRepository.GetConferences(url);

        // we need to pass in conferenceSid separately, because we can't write it from the controller action
        public async Task<Conference> UpdateConference(string conferenceSid, UpdateConferenceOptions options)
        {
            var conference = await ConferenceRepository.GetConference(conferenceSid);
            // TODO: update
            ConferenceCudOperation?.Invoke(this, new ResourceCudOperationEventArgs<Conference>(conference, ResourceCudOperation.Update));
            return conference;
        }

        public Task<ConferenceParticipant> FetchConferenceParticipant(FetchParticipantOptions options)
            => ConferenceParticipantRepository.GetConferenceParticipant(options.PathConferenceSid, options.PathCallSid);

        public Task<ConferenceParticipantsPage> GetConferenceParticipantsPage(Uri url)
            => ConferenceParticipantRepository.GetConferenceParticipants(url);

        // we need to pass in conferenceSid and callSid separately, because we can't write it from the controller action
        public async Task<ConferenceParticipant> UpdateConferenceParticipant(string conferenceSid, string callSid, UpdateParticipantOptions options)
        {
            var participant = await ConferenceParticipantRepository.GetConferenceParticipant(conferenceSid, callSid);
            // TODO: apply logic
            ConferenceParticipantCudOperation?.Invoke(this, new ResourceCudOperationEventArgs<ConferenceParticipant>(participant, ResourceCudOperation.Update));
            return participant;
        }

        public async Task DeleteConferenceParticipant(DeleteParticipantOptions options)
        {
            var participant = await ConferenceParticipantRepository.GetConferenceParticipant(options.PathConferenceSid, options.PathCallSid);
            // TODO: apply logic
            await ConferenceParticipantRepository.DeleteConferenceParticipant(options.PathConferenceSid, options.PathCallSid);
            ConferenceParticipantCudOperation?.Invoke(this, new ResourceCudOperationEventArgs<ConferenceParticipant>(participant, ResourceCudOperation.Delete));
        }

        #endregion

        #region public activity log interface

        public Task<List<ActivityLog>> GetActivityLogs(string sid)
            => ActivityLogRepository.GetActivityLogsForResource(sid);

        #endregion

        #region private implementation

        private async void CallHandler(Uri url, HttpMethod httpMethod, string callSid)
        {
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
                if (httpMethod == HttpMethod.Get)
                {
                    var uriBuilder = new UriBuilder(url);
                    var queryParams = HttpUtility.ParseQueryString(uriBuilder.Query);
                    queryParams.Add(callParams);
                    uriBuilder.Query = queryParams.ToString();
                    url = uriBuilder.Uri;
                    request = new HttpRequestMessage(HttpMethod.Get, url);
                }
                else if (httpMethod == HttpMethod.Post)
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
