using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TwilioLogic.EventModels;
using TwilioLogic.Interfaces;
using TwilioLogic.Models;
using TwilioLogic.Utils;

namespace TwilioLogic
{
    public class CallResources
    {

        private readonly IAccountRepository AccountRepository;
        private readonly ICallResouceRepository CallRepository;

        public event EventHandler<CallResourceChangedEventArgs> CallResourceChanged;

        public CallResources(IAccountRepository accountRepository, ICallResouceRepository callRepository)
        {
            AccountRepository = accountRepository;
            CallRepository = callRepository;
        }

        public async Task<CallResource> CreateIncomingCall(string from, string to, Uri url, HttpMethod httpMethod)
        {
            var call = new CallResource() {
                AccountSid = AccountRepository.GetAccountSid(),
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                Direction = "inbound",
                From = from,
                PhoneNumberSid = await AccountRepository.GetPhoneNumberSid(to),
                Sid = TwilioUtils.CreateSid("CA"),
                Status = "ringing",
                To = to
            };
            await CallRepository.Create(call);
            CallResourceChanged?.Invoke(this, new CallResourceChangedEventArgs(call, true));
            return call;
        }

        public Task<CallResource> GetCallResource(string sid)
            => CallRepository.Get(sid);

        public Task<Page<CallResource>> GetCallResources(ICollection<string> directionFilter = null, ICollection<string> statusFilter = null, long page = 1, long pageSize = long.MaxValue)
            => CallRepository.Get(directionFilter, statusFilter, page, pageSize);

    }
}
