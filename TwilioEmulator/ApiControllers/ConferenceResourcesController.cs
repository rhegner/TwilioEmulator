using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TwilioLogic;
using TwilioLogic.Models;

namespace TwilioEmulator.ApiControllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ConferenceResourcesController : ControllerBase
    {
        private readonly TwilioEngine TwilioEngine;

        public ConferenceResourcesController(TwilioEngine twilioEngine)
        {
            TwilioEngine = twilioEngine;
        }

        [HttpGet("{sid}")]
        public async Task<ActionResult<ConferenceResource>> GetConferenceResource([FromRoute] string sid)
        {
            var conference = await TwilioEngine.GetConferenceResource(sid);
            return conference;
        }

        [HttpGet]
        public async Task<ActionResult<Page<ConferenceResource>>> GetConferenceResources([FromQuery] ICollection<string> statusFilter = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var conferences = await TwilioEngine.GetConferenceResources(statusFilter, page, pageSize);
            return conferences;
        }

    }
}
