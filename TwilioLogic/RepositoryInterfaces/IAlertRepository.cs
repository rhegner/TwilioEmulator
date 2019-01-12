using System;
using System.Threading.Tasks;
using TwilioLogic.Models;
using TwilioLogic.TwilioModels;

namespace TwilioLogic.RepositoryInterfaces
{
    public interface IAlertRepository
    {

        Task CreateAlert(Alert alert);

        Task<Alert> GetAlert(string alertSid);

        Task<PageList<Alert>> GetAlerts(string resourceSidFilter, string logLevelFilter,
            DateTime? messageDateFilter, DateTime? messageDateBeforeFilter, DateTime? messageDateAfterFilter,
            int page, int pageSize, string pageToken);

        Task DeleteAlert(string alertSid);

    }
}
