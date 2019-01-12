using Microsoft.AspNetCore.Mvc.Testing;

namespace ApiTests.Helpers
{
    public class TwilioEmulatorFactory : WebApplicationFactory<TwilioEmulator.Startup> 
    {
    }
}
