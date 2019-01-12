using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TwilioLogic.Interfaces;

namespace TwilioEmulator.TwilioModels
{
    public abstract class ResourcePage<T>
        where T : IResource
    {

        public const int DEFAULT_PAGE_SIZE = 50;
        public const string PAGE_TOKEN_PREFIX = "PA";

        protected ResourcePage(IReadOnlyList<T> resources, bool hasMore, Uri currentUrl)
        {
            Resources = resources ?? new List<T>();

            if (currentUrl != null)
            {
                var previousPageToken = (Resources.Count > 0) ? PAGE_TOKEN_PREFIX + Resources.First().GetSid() : null;
                var nextPageToken = (Resources.Count > 0 && hasMore) ? PAGE_TOKEN_PREFIX + Resources.Last().GetSid() : null;

                var uriBuilder = new UriBuilder(currentUrl);
                var queryParams = HttpUtility.ParseQueryString(uriBuilder.Query);

                if (!int.TryParse(queryParams["Page"], out var page))
                    page = 0;
                Page = page;

                if (!int.TryParse(queryParams["PageSize"], out var pageSize))
                    pageSize = DEFAULT_PAGE_SIZE;
                PageSize = pageSize;

                Start = Page * PageSize;
                End = Start + Math.Max((resources.Count - 1), 0);

                Uri = currentUrl.GetComponents(UriComponents.PathAndQuery, UriFormat.SafeUnescaped);

                queryParams.Set("Page", "0");
                queryParams.Remove("PageToken");
                uriBuilder.Query = queryParams.ToString();
                FirstPageUri = uriBuilder.Uri.GetComponents(UriComponents.PathAndQuery, UriFormat.SafeUnescaped);

                if ((Page > 0) && !string.IsNullOrEmpty(previousPageToken))
                {
                    queryParams.Set("Page", (Page - 1).ToString());
                    queryParams.Set("PageToken", previousPageToken);
                    uriBuilder.Query = queryParams.ToString();
                    PreviousPageUri = uriBuilder.Uri.GetComponents(UriComponents.PathAndQuery, UriFormat.SafeUnescaped);
                }

                if (!string.IsNullOrEmpty(nextPageToken))
                {
                    queryParams.Set("Page", (Page + 1).ToString());
                    queryParams.Set("PageToken", nextPageToken);
                    uriBuilder.Query = queryParams.ToString();
                    PreviousPageUri = uriBuilder.Uri.GetComponents(UriComponents.PathAndQuery, UriFormat.SafeUnescaped);
                }

            }
        }

        public int Page { get; }
        public int PageSize { get; }
        public int Start { get; }
        public int End { get; }

        public string Uri { get; }
        public string FirstPageUri { get; }
        public string PreviousPageUri { get; }
        public string NextPageUri { get; }

        [JsonIgnore]
        public IReadOnlyList<T> Resources { get; }
    }
}
