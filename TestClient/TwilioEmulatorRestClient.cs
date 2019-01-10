using System;
using System.Threading.Tasks;
using Twilio.Clients;
using Twilio.Http;

namespace TestClient
{
    class TwilioEmulatorRestClient : ITwilioRestClient
    {

        public TwilioEmulatorRestClient(ITwilioRestClient wrappedClient, Uri baseUrl)
        {
            WrappedClient = wrappedClient;
            BaseUrl = baseUrl;
        }

        public ITwilioRestClient WrappedClient { get; }

        public Uri BaseUrl { get; }

        public string AccountSid => WrappedClient.AccountSid;

        public string Region => WrappedClient.Region;

        public HttpClient HttpClient => WrappedClient.HttpClient;

        public Response Request(Request request) => WrappedClient.Request(CreateMockRequest(request));

        public Task<Response> RequestAsync(Request request) => WrappedClient.RequestAsync(CreateMockRequest(request));

        private Request CreateMockRequest(Request originalRequest)
        {
            var pathAndQuery = originalRequest.ConstructUrl().GetComponents(UriComponents.PathAndQuery, UriFormat.SafeUnescaped);
            var newUrl = new Uri(BaseUrl, pathAndQuery);
            var mockRequest = new Request(originalRequest.Method, new Uri(BaseUrl, pathAndQuery).ToString());
            mockRequest.PostParams.AddRange(originalRequest.PostParams);
            mockRequest.Username = originalRequest.Username;
            mockRequest.Password = originalRequest.Password;
            return mockRequest;
        }

    }
}
