using System;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {

            TwilioClient.Init("username", "password");
            TwilioClient.SetRestClient(new TwilioEmulatorRestClient(TwilioClient.GetRestClient(), new Uri("http://localhost:58174")));

            var call = CallResource.Create(new PhoneNumber("12345"), new PhoneNumber("34567"));

            Console.WriteLine("Hello World!");
        }
    }
}
