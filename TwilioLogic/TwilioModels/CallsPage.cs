using System;
using System.Collections.Generic;

namespace TwilioLogic.TwilioModels
{
    public class CallsPage : ResourcePage<Call>
    {
        public CallsPage(List<Call> calls, bool hasMore, Uri currentUrl)
            : base(calls, hasMore, currentUrl)
        { }

        public List<Call> Calls { get => Resources; }
    }
}
