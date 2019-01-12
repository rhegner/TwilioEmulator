using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TwilioLogic;
using TwilioLogic.TwilioModels;

namespace TwilioEmulator.ApiControllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CallsController : ControllerBase
    {

        private readonly TwilioEngine TwilioEngine;

        public CallsController(TwilioEngine twilioEngine)
        {
            TwilioEngine = twilioEngine;
        }

        [HttpPost("Incoming")]
        public async Task<ActionResult<Call>> CreateIncomingCall([FromForm] string from, [FromForm] string to, [FromForm] string url, [FromForm] string method)
        {
            var call = await TwilioEngine.CreateIncomingCall(from, to, url, method);
            return call;
        }

    }
}
