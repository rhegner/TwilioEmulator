using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TwilioEmulator.Hubs;
using TwilioEmulator.Services;
using TwilioLogic;
using TwilioLogic.Interfaces;
using TwilioLogic.RepositoryInterfaces;
using TwilioLogic.TwilioModels;
using TwilioMemoryRepositories;

namespace TwilioEmulator
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSignalR();

            services.AddHostedService<NotificationHub>();

            services.AddSingleton<NotificationHub>();
            services.AddSingleton<TwilioEngine>();
            services.AddSingleton<IAccountRepository>(new AccountRepository());
            services.AddSingleton<ICallRepository>(new CallRepository());
            services.AddSingleton<IConferenceRepository>(new ConferenceRepository());
            services.AddSingleton<IConferenceParticipantRepository>(new ConferenceParticipantRepository());
            services.AddSingleton<IActivityLogRepository>(new ActivityLogRepository());

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseSignalR(routes => {
                routes.MapHub<ResourceCudNotificationHub<Call>>("/hubs/calls");
                routes.MapHub<ResourceCudNotificationHub<Conference>>("/hubs/conferences");
                routes.MapHub<ResourceCudNotificationHub<ConferenceParticipant>>("/hubs/conferenceparticipants");
                routes.MapHub<ActivityLogsHub>("/hubs/activitylogs");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
