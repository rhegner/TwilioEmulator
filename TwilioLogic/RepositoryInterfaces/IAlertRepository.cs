using System;
using System.Threading.Tasks;
using TwilioLogic.TwilioModels;

namespace TwilioLogic.RepositoryInterfaces
{
    public interface IAlertRepository
    {

        Task CreateAlert(Alert alert);

        Task<Alert> GetAlert(string alertSid);

        Task<NotificationsPage> GetNotificationsPage(Uri url);

        Task DeleteAlert(string alertSid);

    }
}
