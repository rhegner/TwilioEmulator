using System;
using System.Collections.Generic;
using TwilioLogic.TwilioModels;

namespace TwilioEmulator.TwilioModels
{
    public class ConferencesPage : ResourcePage<Conference>
    {
        public ConferencesPage(IReadOnlyList<Conference> conferences, bool hasMore, Uri currentUrl)
            : base(conferences, hasMore, currentUrl)
        { }

        public IReadOnlyList<Conference> Conferences { get => Resources; }
    }
}
