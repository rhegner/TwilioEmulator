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
            TwilioEngine.ConferenceParticipantCudOperation += TwilioEngine_CudOperation;
            TwilioEngine.AlertCudOperation += TwilioEngine_CudOperation;
            TwilioEngine.ActivityLogCudOperation += TwilioEngine_CudOperation;
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            TwilioEngine.CallCudOperation -= TwilioEngine_CudOperation;
            TwilioEngine.ConferenceCudOperation -= TwilioEngine_CudOperation;
            TwilioEngine.ConferenceParticipantCudOperation -= TwilioEngine_CudOperation;
            TwilioEngine.AlertCudOperation -= TwilioEngine_CudOperation;
            TwilioEngine.ActivityLogCudOperation -= TwilioEngine_CudOperation;
            return Task.CompletedTask;
        }

        private async void TwilioEngine_CudOperation<T>(object sender, ResourceCudOperationEventArgs<T> e)
            where T: class, IResource
        {
            try
            {
                using (var scope = ServiceScopeFactory.CreateScope())
                {
                    var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<ResourceCudNotificationHub<T>, IResourceCudNotificationClient<T>>>();
                    if (e.Resource == null)
                        await hubContext.Clients.All.ResourceCudOperation(null, e.Operation);
                    else
                        await hubContext.Clients.Groups("*", e.Resource.GetTopLevelSid()).ResourceCudOperation(e.Resource, e.Operation);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Could not send CUD operation notifications");
            }
        }

    }
}
