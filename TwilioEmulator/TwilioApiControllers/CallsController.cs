using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;
using TwilioEmulator.Utils;
using TwilioLogic;
using TwilioLogic.TwilioModels;

namespace TwilioEmulator.TwilioApiControllers
{

    [ApiController]
    [Route("/2010-04-01/Accounts/{accountSid}/Calls")]
    public class CallsController : ControllerBase
    {

        private readonly TwilioEngine TwilioEngine;

        public CallsController(TwilioEngine twilioEngine)
        {
            TwilioEngine = twilioEngine;
        }

        [HttpPost(".json")]
        public async Task<ActionResult<Call>> CreateCall([FromRoute] string accountSid, [FromBody] CreateCallOptions createCallOptions)
        {
            createCallOptions.PathAccountSid = accountSid;
            var call = await TwilioEngine.CreateCall(createCallOptions, "2010-04-01");
            return call;
        }

        [HttpGet("{callSid}.json")]
        public async Task<ActionResult<Call>> GetCall([FromRoute] string accountSid, [FromRoute] string callSid)
        {
            var options = new FetchCallOptions(callSid) { PathAccountSid = accountSid };
            var call = await TwilioEngine.FetchCall(options);
            return call;
        }

        [HttpGet(".json")]
        public async Task<ActionResult<CallsPage>> GetCalls()
        {
            var page = await TwilioEngine.GetCallsPage(Request.GetFullRequestUri());
            return page;
        }

        [HttpPost("{callSid}.json")]
        public async Task<ActionResult<Call>> UpdateCall([FromRoute] string accountSid, [FromBody] UpdateCallOptions updateCallOptions)
        {
            updateCallOptions.PathAccountSid = accountSid;
            var call = await TwilioEngine.UpdateCall(updateCallOptions);
            return call;
        }

        [HttpDelete("{callSid}.json")]
        public async Task<ActionResult> DeleteCall([FromRoute] string accountSid, [FromRoute] string callSid)
        {
            var options = new DeleteCallOptions(callSid) { PathAccountSid = accountSid };
            await TwilioEngine.DeleteCall(options);
            return NoContent();
        }
    }
}
