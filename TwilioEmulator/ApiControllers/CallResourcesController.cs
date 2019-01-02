﻿using Microsoft.AspNetCore.Mvc;
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

        private readonly CallResources CallResources;

        public CallResourcesController(CallResources callResources)
        {
            CallResources = callResources;
        }

        [HttpPost("Incoming")]
        public async Task<ActionResult<CallResource>> CreateIncomingCall([FromForm] string from, [FromForm] string to, [FromForm] Uri url, [FromForm] string httpMethod = "post")
        {
            var call = await CallResources.CreateIncomingCall(from, to, url, new HttpMethod(httpMethod));
            return call;
        }

        [HttpGet("{sid}")]
        public async Task<ActionResult<CallResource>> GetCallResource([FromRoute] string sid)
        {
            var call = await CallResources.GetCallResource(sid);
            return call;
        }

        [HttpGet]
        public async Task<ActionResult<Page<CallResource>>> GetCallResources([FromQuery] ICollection<string> directionFilter = null, [FromQuery] ICollection<string> statusFilter = null, [FromQuery] long page = 1, [FromQuery] long pageSize = 20)
        {
            var calls = await CallResources.GetCallResources(directionFilter, statusFilter, page, pageSize);
            return calls;
        }

    }
}
