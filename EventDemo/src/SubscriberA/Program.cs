using System;
using System.Threading.Tasks;
using Rebus.Activation;
using Rebus.Config;
using Shared.Messages;

namespace SubscriberA
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var container = new BuiltinHandlerActivator())
            {
                container.Handle<HelloWorldPublishedEvent>((b, context, msg) =>
                {
                    Console.WriteLine($"Subscriber A - Retrieved {msg.GetType().Namespace}... Say Hello From {msg.From}");
                    return Task.CompletedTask;;
                });

                var bus = Configure.With(container)
                    .Logging(l => l.ColoredConsole())
                    .Transport(t => t.UseAzureServiceBus(connectionStringNameOrConnectionString: Credentials.ServiceBusConnectionString, "SubScriberA-input"))
                    .Start();

                bus.Subscribe<HelloWorldPublishedEvent>().Wait();

                Console.ReadLine();
            }
        }
    }
}
