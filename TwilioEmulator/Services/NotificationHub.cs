using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TwilioEmulator.Hubs;
using TwilioLogic;

namespace TwilioEmulator.Services
{
    public class NotificationHub : IHostedService
    {

        private readonly IServiceScopeFactory ServiceScopeFactory;
        private readonly CallResources CallResources;
        private readonly ILogger<NotificationHub> Logger;

        public NotificationHub(IServiceScopeFactory serviceScopeFactory, CallResources callResources, ILogger<NotificationHub> logger)
        {
            ServiceScopeFactory = serviceScopeFactory;
            CallResources = callResources;
            Logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            CallResources.CallResourceChanged += CallResources_CallResourceChanged;
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            CallResources.CallResourceChanged -= CallResources_CallResourceChanged;
            return Task.CompletedTask;
        }

        private async void CallResources_CallResourceChanged(object sender, TwilioLogic.EventModels.CallResourceChangedEventArgs e)
        {
            try
            {
                using (var scope = ServiceScopeFactory.CreateScope())
                {
                    var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<CallResourcesHub, ICallResourcesClient>>();
                    await hubContext.Clients.All.CallResourceUpdate(e.CallResource, e.IsNew);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Could not send call resource update notifications");
            }
        }
    }
}
