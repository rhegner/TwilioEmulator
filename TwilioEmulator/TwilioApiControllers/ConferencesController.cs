using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TwilioEmulator.TwilioModels;
using TwilioEmulator.Utils;
using TwilioLogic;
using TwilioLogic.TwilioModels;

namespace TwilioEmulator.TwilioApiControllers
{

    [ApiController]
    [Route("/" + API_VERSION + "/Accounts/{accountSid}")]
    public class ConferencesController : ControllerBase
    {
        private const string API_VERSION = "2010-04-01";

        private readonly TwilioEngine TwilioEngine;

        public ConferencesController(TwilioEngine twilioEngine)
        {
            TwilioEngine = twilioEngine;
        }

        [HttpGet("Conferences/{conferenceSid}.json")]
        public async Task<ActionResult<Conference>> GetConference([FromRoute] string accountSid, [FromRoute] string conferenceSid)
        {
            var conference = await TwilioEngine.GetConference(conferenceSid);
            return conference;
        }

        [HttpGet("Conferences.json")]
        public async Task<ActionResult<ConferencesPage>> GetConferences([FromRoute] string accountSid, 
            [FromQuery] DateTime? DateCreated, [FromQuery(Name = "DateCreated<")] DateTime? DateCreatedBefore, [FromQuery(Name = "DateCreated>")] DateTime? DateCreatedAfter,
            [FromQuery] DateTime? DateUpdated, [FromQuery(Name = "DateUpdated<")] DateTime? DateUpdatedBefore, [FromQuery(Name = "DateUpdated>")] DateTime? DateUpdatedAfter,
            [FromQuery] string FriendlyName, [FromQuery] string[] Status,
            [FromQuery] int Page = 0, [FromQuery] int PageSize = 50, [FromQuery] string PageToken = null)
        {
            var conferences = await TwilioEngine.GetConferences(DateCreated, DateCreatedBefore, DateCreatedAfter,
                DateUpdated, DateUpdatedBefore, DateUpdatedAfter,
                FriendlyName, Status,
                Page, PageSize, PageToken);
            var conferencesPage = new ConferencesPage(conferences, conferences.HasMore, Request.GetFullRequestUri());
            return conferencesPage;
        }

        [HttpPost("Conferences/{conferenceSid}.json")]
        public async Task<ActionResult<Conference>> UpdateConference([FromRoute] string accountSid, [FromRoute] string conferenceSid)
        {
            var conference = await TwilioEngine.UpdateConference(conferenceSid);
            return conference;
        }

        [HttpGet("Conferences/{conferenceSid}/Participants/{callSid}.json")]
        public async Task<ActionResult<ConferenceParticipant>> GetConferenceParticipant([FromRoute] string accountSid, [FromRoute] string conferenceSid, [FromRoute] string callSid)
        {
            var participant = await TwilioEngine.GetConferenceParticipant(conferenceSid, callSid);
            return participant;
        }

        [HttpGet("Conferences/{conferenceSid}/Participants.json")]
        public async Task<ActionResult<ConferenceParticipantsPage>> GetConferenceParticipants([FromRoute] string accountSid, [FromRoute] string conferenceSid,
            [FromQuery] bool? Muted, [FromQuery] bool? Hold,
            [FromQuery] int Page = 0, [FromQuery] int PageSize = 50, [FromQuery] string PageToken = null)
        {
            var participants = await TwilioEngine.GetConferenceParticipants(conferenceSid, Muted, Hold,
                Page, PageSize, PageToken);
            var participantsPage = new ConferenceParticipantsPage(participants, participants.HasMore, Request.GetFullRequestUri());
            return participantsPage;
        }

        [HttpPost("Conferences/{conferenceSid}/Participants/{callSid}.json")]
        public async Task<ActionResult<ConferenceParticipant>> UpdateConferenceParticipant([FromRoute] string accountSid, [FromRoute] string conferenceSid, [FromRoute] string callSid)
        {
            var conference = await TwilioEngine.UpdateConferenceParticipant(conferenceSid, callSid);
            return conference;
        }

        [HttpDelete("Conferences/{conferenceSid}/Participants/{callSid}.json")]
        public async Task<ActionResult> DeleteConferenceParticipant([FromRoute] string accountSid, [FromRoute] string conferenceSid, [FromRoute] string callSid)
        {
            await TwilioEngine.DeleteConferenceParticipant(conferenceSid, callSid);
            return NoContent();
        }

    }
}
