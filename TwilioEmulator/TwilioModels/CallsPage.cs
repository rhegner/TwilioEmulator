using System;
using System.Collections.Generic;
using TwilioLogic.TwilioModels;

namespace TwilioEmulator.TwilioModels
{
    public class CallsPage : ResourcePage<Call>
    {
        public CallsPage(IReadOnlyList<Call> calls, bool hasMore, Uri currentUrl)
            : base(calls, hasMore, currentUrl)
        { }

        public IReadOnlyList<Call> Calls { get => Resources; }
    }
}
