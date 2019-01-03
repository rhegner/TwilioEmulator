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
            services.AddSingleton<ICallResouceRepository>(new CallResourceRepository());
            services.AddSingleton<IConferenceResourceRepository>(new ConferenceResourceRepository());
            services.AddSingleton<IApiCallRepository>(new ApiCallRepository());
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
                routes.MapHub<CallResourcesHub>("/hubs/callresources");
                routes.MapHub<ConferenceResourcesHub>("/hubs/conferenceresources");
                routes.MapHub<ApiCallsHub>("/hubs/apicalls");
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
