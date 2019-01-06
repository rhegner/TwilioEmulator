using System.Collections.Concurrent;
using System.Threading.Tasks;
using TwilioLogic.RepositoryInterfaces;
using TwilioLogic.Utils;

namespace TwilioMemoryRepositories
{
    public class AccountRepository : IAccountRepository
    {

        private readonly ConcurrentDictionary<string, string> PhoneNumberSids = new ConcurrentDictionary<string, string>();
        private readonly string AccountSid;

        public AccountRepository(string accountSid = null)
        {
            AccountSid = accountSid ?? TwilioUtils.CreateSid("AC");
        }

        public string GetAccountSid()
            => AccountSid;

        public Task<string> GetPhoneNumberSid(string phoneNumber)
            => Task.FromResult(PhoneNumberSids.GetOrAdd(phoneNumber, (_) => TwilioUtils.CreateSid("PN")));

    }
}
