using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TwilioEmulator.Hubs;
using TwilioLogic;
using TwilioLogic.EventModels;
using TwilioLogic.Interfaces;

namespace TwilioEmulator.Services
{
    public class NotificationHub : IHostedService
    {

        private readonly IServiceScopeFactory ServiceScopeFactory;
        private readonly TwilioEngine TwilioEngine;
        private readonly ILogger<NotificationHub> Logger;

        public NotificationHub(IServiceScopeFactory serviceScopeFactory, TwilioEngine twilioEngine, ILogger<NotificationHub> logger)
        {
            ServiceScopeFactory = serviceScopeFactory;
            TwilioEngine = twilioEngine;
            Logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            TwilioEngine.CallCudOperation += TwilioEngine_CudOperation;
            TwilioEngine.ConferenceCudOperation += TwilioEngine_CudOperation;
            TwilioEngine.NewActivityLog += TwilioEngine_NewActivityLog;
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            TwilioEngine.CallCudOperation -= TwilioEngine_CudOperation;
            TwilioEngine.ConferenceCudOperation -= TwilioEngine_CudOperation;
            TwilioEngine.NewActivityLog -= TwilioEngine_NewActivityLog;
            return Task.CompletedTask;
        }

        private async void TwilioEngine_CudOperation<T>(object sender, ResourceCudOperationEventArgs<T> e)
            where T: IResource
        {
            try
            {
                using (var scope = ServiceScopeFactory.CreateScope())
                {
                    var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<ResourceCudNotificationHub<T>, IResourceCudNotificationClient<T>>>();
                    await hubContext.Clients.Groups("*", e.Resource.GetTopLevelSid()).ResourceCudOperation(e.Resource, e.Operation);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Could not send CUD operation notifications");
            }
        }

        private async void TwilioEngine_NewActivityLog(object sender, TwilioLogic.EventModels.NewActivityLogEventArgs e)
        {
            try
            {
                using (var scope = ServiceScopeFactory.CreateScope())
                {
                    var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<ActivityLogsHub, IActivityLogsClient>>();
                    await hubContext.Clients.All.NewActivityLog(e.ActivityLog);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Could not send new activity log notifications");
            }
        }

    }
}
