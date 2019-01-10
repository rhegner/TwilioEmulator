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

        [HttpPost(".{ext?}")]
        public async Task<ActionResult<Call>> CreateCall([FromRoute] string accountSid, [FromRoute] string ext, [FromForm] CreateCallOptions createCallOptions)
        {
            createCallOptions.PathAccountSid = accountSid;
            var call = await TwilioEngine.CreateCall(createCallOptions, "2010-04-01");
            return call;
        }

        [HttpGet("{callSid}.{ext?}")]
        public async Task<ActionResult<Call>> GetCall([FromRoute] string accountSid, [FromRoute] string callSid, [FromRoute] string ext)
        {
            var options = new FetchCallOptions(callSid) { PathAccountSid = accountSid };
            var call = await TwilioEngine.FetchCall(options);
            return call;
        }

        [HttpGet(".{ext?}")]
        public async Task<ActionResult<CallsPage>> GetCalls([FromRoute] string ext)
        {
            var page = await TwilioEngine.GetCallsPage(Request.GetFullRequestUri());
            return page;
        }

        [HttpPost("{callSid}.{ext?}")]
        public async Task<ActionResult<Call>> UpdateCall([FromRoute] string accountSid, [FromRoute] string callSid, [FromRoute] string ext, [FromForm] UpdateCallOptions updateCallOptions)
        {
            updateCallOptions.PathAccountSid = accountSid;
            var call = await TwilioEngine.UpdateCall(callSid, updateCallOptions);
            return call;
        }

        [HttpDelete("{callSid}.{ext?}")]
        public async Task<ActionResult> DeleteCall([FromRoute] string accountSid, [FromRoute] string callSid, [FromRoute] string ext)
        {
            var options = new DeleteCallOptions(callSid) { PathAccountSid = accountSid };
            await TwilioEngine.DeleteCall(options);
            return NoContent();
        }
    }
}
