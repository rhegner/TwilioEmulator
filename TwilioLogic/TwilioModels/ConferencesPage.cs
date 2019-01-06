using System;
using System.Collections.Generic;

namespace TwilioLogic.TwilioModels
{
    public class ConferencesPage : ResourcePage<Conference>
    {
        public ConferencesPage(List<Conference> conferences, bool hasMore, Uri currentUrl)
            : base(conferences, hasMore, currentUrl)
        { }

        public List<Conference> Conferences { get => Resources; }
    }
}
