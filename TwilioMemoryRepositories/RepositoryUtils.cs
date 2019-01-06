using System;
using System.Collections.Specialized;
using System.Linq;
using TwilioLogic.Interfaces;
using TwilioLogic.TwilioModels;

namespace TwilioMemoryRepositories
{
    internal static class RepositoryUtils
    {
        public static string[] GetStringArray(this NameValueCollection nvc, string key)
            => nvc.Get(key)?.Split(',').ToArray();

        public static DateTime? GetDateTime(this NameValueCollection nvc, string key)
            => DateTime.TryParse(nvc.Get(key), out var val) ? (DateTime?)val : null;

        public static bool? GetBool(this NameValueCollection nvc, string key)
            => bool.TryParse(nvc.Get(key), out var val) ? (bool?)val : null;

        public static int GetPage(this NameValueCollection nvc)
            => int.TryParse(nvc.Get("Page"), out var page) && (page >= 0) ? page : 0;

        public static int GetPageSize(this NameValueCollection nvc)
            => int.TryParse(nvc.Get("PageSize"), out var pageSize) && (pageSize >= 1) ? pageSize : ResourcePage<IResource>.DEFAULT_PAGE_SIZE;

        public static string GetPageToken(this NameValueCollection nvc)
        {
            var prefixedToken = nvc.Get("PageToken");
            if (string.IsNullOrEmpty(prefixedToken))
            {
                return null;
            }
            else
            {
                if (!prefixedToken.StartsWith(ResourcePage<IResource>.PAGE_TOKEN_PREFIX))
                    throw new Exception($"Unexpected page token {prefixedToken}");
                return prefixedToken.Substring(ResourcePage<IResource>.PAGE_TOKEN_PREFIX.Length);
            }
        }
    }
}
