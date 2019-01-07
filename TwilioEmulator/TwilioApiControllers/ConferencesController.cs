using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Rest.Api.V2010.Account.Conference;
using TwilioEmulator.Utils;
using TwilioLogic;
using TwilioLogic.TwilioModels;

namespace TwilioEmulator.TwilioApiControllers
{

    [ApiController]
    [Route("/2010-04-01/Accounts/{accountSid}/Conferences")]
    public class ConferencesController : ControllerBase
    {

        private readonly TwilioEngine TwilioEngine;

        public ConferencesController(TwilioEngine twilioEngine)
        {
            TwilioEngine = twilioEngine;
        }

        [HttpGet("{conferenceSid}.json")]
        public async Task<ActionResult<Conference>> GetConference([FromRoute] string accountSid, [FromRoute] string conferenceSid)
        {
            var options = new FetchConferenceOptions(conferenceSid) { PathAccountSid = accountSid };
            var conference = await TwilioEngine.FetchConference(options);
            return conference;
        }

        [HttpGet(".json")]
        public async Task<ActionResult<ConferencesPage>> GetConferences()
        {
            var page = await TwilioEngine.GetConferencesPage(Request.GetFullRequestUri());
            return page;
        }

        [HttpPost("{conferenceSid}.json")]
        public async Task<ActionResult<Conference>> UpdateConference([FromRoute] string accountSid, [FromRoute] string conferenceSid, [FromBody] UpdateConferenceOptions updateConferenceOptions)
        {
            updateConferenceOptions.PathAccountSid = accountSid;
            var conference = await TwilioEngine.UpdateConference(conferenceSid, updateConferenceOptions);
            return conference;
        }

        [HttpGet("{conferenceSid}/Participants/{callSid}.json")]
        public async Task<ActionResult<ConferenceParticipant>> GetConferenceParticipant([FromRoute] string accountSid, [FromRoute] string conferenceSid, [FromRoute] string callSid)
        {
            var options = new FetchParticipantOptions(conferenceSid, callSid) { PathAccountSid = accountSid };
            var participant = await TwilioEngine.FetchConferenceParticipant(options);
            return participant;
        }

        [HttpGet("{conferenceSid}/Participants.json")]
        public async Task<ActionResult<ConferenceParticipantsPage>> GetConferenceParticipants()
        {
            var participants = await TwilioEngine.GetConferenceParticipantsPage(Request.GetFullRequestUri());
            return participants;
        }

        [HttpPost("{conferenceSid}/Participants/{callSid}.json")]
        public async Task<ActionResult<ConferenceParticipant>> UpdateConferenceParticipant([FromRoute] string accountSid, [FromRoute] string conferenceSid, [FromRoute] string callSid, [FromBody] UpdateParticipantOptions updateParticipantOptions)
        {
            updateParticipantOptions.PathAccountSid = accountSid;
            var conference = await TwilioEngine.UpdateConferenceParticipant(conferenceSid, callSid, updateParticipantOptions);
            return conference;
        }

        [HttpDelete("{conferenceSid}/Participants/{callSid}.json")]
        public async Task<ActionResult> DeleteConferenceParticipant([FromRoute] string accountSid, [FromRoute] string conferenceSid, [FromRoute] string callSid)
        {
            var options = new DeleteParticipantOptions(conferenceSid, callSid) { PathAccountSid = accountSid };
            await TwilioEngine.DeleteConferenceParticipant(options);
            return NoContent();
        }

    }
}
