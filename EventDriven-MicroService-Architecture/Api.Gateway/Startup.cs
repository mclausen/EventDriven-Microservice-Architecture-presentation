using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notifications.Messages.External;
using Profile.Message.External;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Rebus.ServiceProvider;

namespace Api.Gateway
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddRebus(configure => configure
                .Logging(l => l.ColoredConsole())
                .Transport(t => t.UseAzureServiceBusAsOneWayClient(
                    connectionStringNameOrConnectionString: SharedKernel.Credentials.ServiceBus.ConnectionString))
                .Routing(x => x.TypeBased()
                    .Map<AcceptFriendRequestCommand>("social-notifications-input")
                    .Map<CreateFriendRequestCommand>("social-notifications-input")
                    .Map<CreateProfileCommand>("social-profile-input")
                    .Map<UpdateProfileCommand>("social-profile-input")
                )
            );

            services.AddHttpClient();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRebus();
            app.UseMvc();
        }
    }
}
