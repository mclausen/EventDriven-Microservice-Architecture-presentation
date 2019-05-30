using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Notification.Domain;
using Rebus.Bus;
using Rebus.ServiceProvider;
using SharedKernel;

namespace Notification.Worker
{
    public class Service : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        
        /// <summary>
        /// Events that this worker subscribes to
        /// </summary>
        private static readonly Type[] EventSubscriptionTypes = {
            // typeof(MyFeatureHasHappenedEvents)
        };

        public Service(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _serviceProvider.UseRebus(async bus =>
            {
                await ConfigureRebusSubscriptions(bus);
            });

            DocumentDbRepository<FriendShipRequestNotification>.Initialize();
        }

        private static async Task ConfigureRebusSubscriptions(IBus bus)
        {
            foreach (var subscriptionType in EventSubscriptionTypes)
            {
                await bus.Subscribe(subscriptionType);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
