using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TwilioEmulator.TwilioModels;
using TwilioEmulator.Utils;
using TwilioLogic;
using TwilioLogic.TwilioModels;

namespace TwilioEmulator.TwilioApiControllers
{

    [ApiController]
    [Route("/" + API_VERSION + "/Accounts/{accountSid}")]
    public class CallsController : ControllerBase
    {
        private const string API_VERSION = "2010-04-01";

        private readonly TwilioEngine TwilioEngine;

        public CallsController(TwilioEngine twilioEngine)
        {
            TwilioEngine = twilioEngine;
        }

        [HttpPost("Calls.json")]
        public async Task<ActionResult<Call>> CreateCall([FromRoute] string accountSid,
            [FromForm] string From, [FromForm] string Method, [FromForm] string To, [FromForm] string Url)
        {
            var call = await TwilioEngine.CreateCall(accountSid, API_VERSION, From, Method, To, Url);
            return call;
        }

        [HttpGet("Calls/{callSid}.json")]
        public async Task<ActionResult<Call>> GetCall([FromRoute] string accountSid, [FromRoute] string callSid)
        {
            var call = await TwilioEngine.GetCall(callSid);
            return call;
        }

        [HttpGet("Calls.json")]
        public async Task<ActionResult<CallsPage>> GetCalls([FromRoute] string accountSid,
            [FromQuery] string To, [FromQuery] string From, [FromQuery] string ParentCallSid, [FromQuery] string[] Status,
            [FromQuery] DateTime? StartTime, [FromQuery(Name = "StartTime<")] DateTime? StartTimeBefore, [FromQuery(Name = "StartTime>")] DateTime? StartTimeAfter,
            [FromQuery] DateTime? EndTime, [FromQuery(Name = "EndTime<")] DateTime? EndTimeBefore, [FromQuery(Name = "EndTime>")] DateTime? EndTimeAfter,
            [FromQuery] int Page = 0, [FromQuery] int PageSize = 50, [FromQuery] string PageToken = null)
        {
            var calls = await TwilioEngine.GetCalls(To, From, ParentCallSid, Status,
                StartTime, StartTimeBefore, StartTimeAfter,
                EndTime, EndTimeBefore, EndTimeAfter,
                Page, PageSize, CallsPage.TrimTokenPrefix(PageToken));
            var callsPage = new CallsPage(calls, calls.HasMore, Request.GetFullRequestUri());
            return callsPage;
        }

        [HttpPost("Calls/{callSid}.json")]
        public async Task<ActionResult<Call>> UpdateCall([FromRoute] string accountSid, [FromRoute] string callSid)
        {
            var call = await TwilioEngine.UpdateCall(callSid);
            return call;
        }

        [HttpDelete("Calls/{callSid}.json")]
        public async Task<ActionResult> DeleteCall([FromRoute] string accountSid, [FromRoute] string callSid)
        {
            await TwilioEngine.DeleteCall(callSid);
            return NoContent();
        }
    }
}
