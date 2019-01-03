using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using TwilioLogic.EventModels;
using TwilioLogic.Interfaces;
using TwilioLogic.Models;
using TwilioLogic.Utils;

namespace TwilioLogic
{
    public class CallResources
    {

        private readonly IAccountRepository AccountRepository;
        private readonly ICallResouceRepository CallRepository;
        private readonly IApiCallRepository ApiCallRepository;
        private readonly ILogger<CallResources> Logger;

        public event EventHandler<CallResourceChangedEventArgs> CallResourceChanged;
        public event EventHandler<NewApiCallEventArgs> NewApiCall;

        public CallResources(IAccountRepository accountRepository, ICallResouceRepository callRepository, IApiCallRepository apiCallRepository,
            ILogger<CallResources> logger)
        {
            AccountRepository = accountRepository;
            CallRepository = callRepository;
            ApiCallRepository = apiCallRepository;
            Logger = logger;
        }

        public async Task<CallResource> CreateIncomingCall(string from, string to, Uri url, HttpMethod httpMethod)
        {
            var call = new CallResource() {
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
            await CallRepository.Create(call);
            CallResourceChanged?.Invoke(this, new CallResourceChangedEventArgs(call, true));
            CallHandler(url, httpMethod, call.Sid);
            return call;
        }

        public Task<CallResource> GetCallResource(string callSid)
            => CallRepository.Get(callSid);

        public Task<Page<CallResource>> GetCallResources(ICollection<string> directionFilter = null, ICollection<string> statusFilter = null, long page = 1, long pageSize = long.MaxValue)
            => CallRepository.Get(directionFilter, statusFilter, page, pageSize);

        public Task<List<ApiCall>> GetApiCalls(string callSid)
            => ApiCallRepository.GetApiCallsForResource(callSid);

        private async void CallHandler(Uri url, HttpMethod httpMethod, string callSid)
        {
            try
            {
                var apiCall = new ApiCall()
                {
                    ApiCallId = Guid.NewGuid(),
                    Sid = callSid,
                    Direction = ApiCallDirection.FromEmulator,
                    Type = ApiCallType.IncomingCallCallback,
                    HttpMethod = httpMethod.ToString()
                };

                var call = await CallRepository.Get(callSid);
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
                    apiCall.RequestContentType = request.Content.Headers.ContentType.ToString();
                    apiCall.RequestContent = await request.Content.ReadAsStringAsync();
                }
                else
                {
                    throw new NotSupportedException($"{httpMethod} is not supported");
                }

                apiCall.Url = url;

                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        apiCall.RequestTimestamp = DateTime.UtcNow;
                        using (var response = await httpClient.SendAsync(request))
                        {
                            apiCall.ResponseTimestamp = DateTime.UtcNow;
                            if (response.Content != null)
                            {
                                apiCall.ResponseContentType = response.Content.Headers.ContentType.ToString();
                                apiCall.ResponseContent = await response.Content.ReadAsStringAsync();
                            }
                            apiCall.ResponseStatusCode = (int)response.StatusCode;
                        }
                    }
                }
                catch (Exception ex)
                {
                    apiCall.ResponseStatusCode = -1;
                    Logger.LogError(ex, "Error sending callback request");
                }

                await ApiCallRepository.CreateApiCall(apiCall);
                NewApiCall?.Invoke(this, new NewApiCallEventArgs(apiCall));

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error invoking callback");
            }
        }

    }
}
