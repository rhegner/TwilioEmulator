using ApiTests.Helpers;
using System;
using Twilio.Clients;
using Twilio.Types;
using Xunit;

namespace ApiTests
{
    public class UnitTest1 : IClassFixture<TwilioEmulatorFactory>
    {

        public UnitTest1(TwilioEmulatorFactory factory)
        {
            var client = factory.CreateClient();
            Twilio.TwilioClient.SetRestClient(new TwilioRestClient("user", "password", "accountSid", "region", new Twilio.Http.SystemNetHttpClient(client)));
        }

        [Fact]
        public void Test1()
        {
            Twilio.Rest.Api.V2010.Account.CallResource.Create(new PhoneNumber("+41788831118"), new PhoneNumber("+41435051118"));
        }

        [Fact]
        public void Test2()
        {
            Twilio.Rest.Api.V2010.Account.CallResource.Create(new PhoneNumber("+41788831118"), new PhoneNumber("+41435051118"));
        }
    }
}
