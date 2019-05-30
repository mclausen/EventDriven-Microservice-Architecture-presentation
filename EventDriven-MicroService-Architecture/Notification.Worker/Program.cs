using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rebus.Config;
using Rebus.Retry.Simple;
using Rebus.ServiceProvider;

namespace Notification.Worker
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddEnvironmentVariables();
                })
                .ConfigureServices((hostingContext, services) =>
                {
                    services.AddHttpClient();

                    // Rebus
                    services.AutoRegisterHandlersFromAssemblyOf<Program>();
                    services.AddRebus(configure => configure
                        .Logging(l => l.ColoredConsole())
                        .Transport(t => t.UseAzureServiceBus(
                            connectionStringNameOrConnectionString: SharedKernel.Credentials.ServiceBus.ConnectionString,
                            inputQueueAddress: "social-notifications-input"))
                        .Options(o => o.SimpleRetryStrategy(errorQueueAddress: "social-notifications-error"))
                    );
                    services.AddSingleton<IHostedService, Service>();
                })
                .ConfigureHostConfiguration(config => { });

            await builder.RunConsoleAsync();
        }
    }
}
