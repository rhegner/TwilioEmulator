using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Twilio.Clients;
using Twilio.Types;
using Xunit;

namespace ApiTests.Helpers
{
    public class TwilioEmulatorTestBase : IClassFixture<TwilioEmulatorFactory>
    {

        protected static readonly PhoneNumber TEST_FROM_NUMBER = new PhoneNumber("+41788831118");
        protected static readonly PhoneNumber TEST_TO_NUMBER = new PhoneNumber("+41435051119");

        protected TwilioEmulatorFactory Factory { get; }

        public TwilioEmulatorTestBase(TwilioEmulatorFactory factory)
        {
            Factory = factory;
            var client = Factory.CreateClient();
            Twilio.TwilioClient.SetRestClient(new TwilioRestClient("test-user", "test-password", "ACf59308bf40cac3aedb045a963c703e96", null, new Twilio.Http.SystemNetHttpClient(client)));
        }

        protected Task ClearDatabase()
        {
            var twilioEngine = Factory.Server.Host.Services.GetRequiredService<TwilioLogic.TwilioEngine>();
            return twilioEngine.ClearDatabase();
        }

    }
}
