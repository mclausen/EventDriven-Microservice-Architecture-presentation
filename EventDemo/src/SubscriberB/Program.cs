using System;
using System.Threading.Tasks;
using Rebus.Activation;
using Rebus.Config;
using Shared.Messages;

namespace SubscriberB
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var container = new BuiltinHandlerActivator())
            {
                container.Handle<HelloWorldPublishedEvent>((b, context, msg) =>
                {
                    Console.WriteLine($"Subscriber A - {msg.GetType().Namespace}... Say Hello From {msg.From}");
                    return Task.CompletedTask;;
                });

                var bus = Configure.With(container)
                    .Logging(l => l.ColoredConsole())
                    .Transport(t => t.UseAzureServiceBus(connectionStringNameOrConnectionString: Credentials.ServiceBusConnectionString, "SubscriberB-input"))
                    .Start();

                bus.Subscribe<HelloWorldPublishedEvent>().Wait();

                Console.ReadLine();
            }
        }
    }
}
