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
            TwilioEngine.CallResourceChanged += CallResources_CallResourceChanged;
            TwilioEngine.ConferenceResourceChanged += TwilioEngine_ConferenceResourceChanged;
            TwilioEngine.NewApiCall += CallResources_NewApiCall;
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            TwilioEngine.CallResourceChanged -= CallResources_CallResourceChanged;
            TwilioEngine.ConferenceResourceChanged -= TwilioEngine_ConferenceResourceChanged;
            TwilioEngine.NewApiCall -= CallResources_NewApiCall;
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

        private async void TwilioEngine_ConferenceResourceChanged(object sender, TwilioLogic.EventModels.ConferenceResourceChangedEventArgs e)
        {
            try
            {
                using (var scope = ServiceScopeFactory.CreateScope())
                {
                    var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<ConferenceResourcesHub, IConferenceResourcesClient>>();
                    await hubContext.Clients.All.ConferenceResourceUpdate(e.ConferenceResource);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Could not send conference resource update notifications");
            }
        }

        private async void CallResources_NewApiCall(object sender, TwilioLogic.EventModels.NewApiCallEventArgs e)
        {
            try
            {
                using (var scope = ServiceScopeFactory.CreateScope())
                {
                    var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<ApiCallsHub, IApiCallsClient>>();
                    await hubContext.Clients.All.NewApiCall(e.ApiCall);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Could not send new api call notifications");
            }
        }
    }
}
