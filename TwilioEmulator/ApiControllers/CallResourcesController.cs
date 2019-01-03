using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TwilioLogic;
using TwilioLogic.Models;

namespace TwilioEmulator.ApiControllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CallResourcesController : ControllerBase
    {

        private readonly TwilioEngine TwilioEngine;

        public CallResourcesController(TwilioEngine twilioEngine)
        {
            TwilioEngine = twilioEngine;
        }

        [HttpPost("Incoming")]
        public async Task<ActionResult<CallResource>> CreateIncomingCall([FromForm] string from, [FromForm] string to, [FromForm] Uri url, [FromForm] string httpMethod = "post")
        {
            var call = await TwilioEngine.CreateIncomingCall(from, to, url, new HttpMethod(httpMethod));
            return call;
        }

        [HttpGet("{callSid}")]
        public async Task<ActionResult<CallResource>> GetCallResource([FromRoute] string callSid)
        {
            var call = await TwilioEngine.GetCallResource(callSid);
            return call;
        }

        [HttpGet]
        public async Task<ActionResult<Page<CallResource>>> GetCallResources([FromQuery] ICollection<string> directionFilter = null, [FromQuery] ICollection<string> statusFilter = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var calls = await TwilioEngine.GetCallResources(directionFilter, statusFilter, page, pageSize);
            return calls;
        }

        [HttpGet("{callSid}/ApiCalls")]
        public async Task<ActionResult<List<ApiCall>>> GetApiCalls([FromRoute] string callSid)
        {
            var apiCalls = await TwilioEngine.GetApiCalls(callSid);
            return apiCalls;
        }

    }
}
