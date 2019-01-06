using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TwilioLogic;
using TwilioLogic.Models;

namespace TwilioEmulator.ApiControllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ResourcesController : ControllerBase
    {
        private readonly TwilioEngine TwilioEngine;

        public ResourcesController(TwilioEngine twilioEngine)
        {
            TwilioEngine = twilioEngine;
        }

        [HttpGet("{sid}/ActivityLogs")]
        public async Task<ActionResult<List<ActivityLog>>> GetActivityLogs([FromRoute] string sid)
        {
            var activityLogs = await TwilioEngine.GetActivityLogs(sid);
            return activityLogs;
        }
    }
}
