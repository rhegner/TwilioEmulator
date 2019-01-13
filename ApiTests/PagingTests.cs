using ApiTests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;
using Xunit;

namespace ApiTests
{
    public class PagingTests : TwilioEmulatorTestBase
    {

        public PagingTests(TwilioEmulatorFactory factory)
            : base(factory)
        { }

        [Theory]
        [InlineData(0, 5)]
        [InlineData(3, 5)]
        [InlineData(5, 5)]
        [InlineData(13, 5)]
        [InlineData(15, 5)]
        public async Task CallsPagingTest(int numberOfCalls, int pageSize)
        {
            await ClearDatabase();

            var callSids1 = new List<string>();
            var callSids2 = new List<string>();

            for (int i = 0; i < numberOfCalls; i++)
            {
                var call = await CallResource.CreateAsync(TEST_TO_NUMBER, TEST_FROM_NUMBER);
                callSids1.Insert(0, call.Sid);
                callSids2.Insert(0, call.Sid);
                await Task.Delay(1);
            }

            var callsSet1 = await CallResource.ReadAsync(pageSize: pageSize);

            // we now have the first page in memory. add a newer calls to make sure this does not disturb the existing pagination
            for (int i = 0; i < 3; i++)
            {
                var call = await CallResource.CreateAsync(TEST_TO_NUMBER, TEST_FROM_NUMBER);
                callSids2.Insert(0, call.Sid);
                await Task.Delay(1);
            }

            var calls1 = callsSet1.ToList();
            Assert.Collection(calls1, callSids1.Select<string, Action<CallResource>>(sid => { return (CallResource call) => Assert.Equal(sid, call.Sid); }).ToArray());

            var callsSet2 = await CallResource.ReadAsync(pageSize: pageSize);
            var calls2 = callsSet2.ToList();
            Assert.Collection(calls2, callSids2.Select<string, Action<CallResource>>(sid => { return (CallResource call) => Assert.Equal(sid, call.Sid); }).ToArray());

        }

    }
}
