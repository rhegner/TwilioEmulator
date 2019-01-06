using System.Threading.Tasks;

namespace TwilioLogic.RepositoryInterfaces
{
    public interface IAccountRepository
    {

        string GetAccountSid();

        Task<string> GetPhoneNumberSid(string phoneNumber);

    }
}
