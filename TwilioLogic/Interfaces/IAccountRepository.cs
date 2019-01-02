using System.Threading.Tasks;

namespace TwilioLogic.Interfaces
{
    public interface IAccountRepository
    {

        string GetAccountSid();

        Task<string> GetPhoneNumberSid(string phoneNumber);

    }
}
