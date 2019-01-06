using Microsoft.AspNetCore.Http;
using System;

namespace TwilioEmulator.Utils
{
    public static class ControllerExtensions
    {

        public static Uri GetFullRequestUri(this HttpRequest request)
        {
            // There are many approaches for this:
            // https://stackoverflow.com/questions/28120222/get-raw-url-from-microsoft-aspnet-http-httprequest
            var uriBuilder = new UriBuilder
            {
                Scheme = request.Scheme,
                Host = request.Host.Host,
                Port = request.Host.Port.GetValueOrDefault(80),
                Path = request.Path.ToString(),
                Query = request.QueryString.ToString()
            };
            return uriBuilder.Uri;
        }

    }
}
