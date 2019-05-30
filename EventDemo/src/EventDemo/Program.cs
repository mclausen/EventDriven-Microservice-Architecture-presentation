using System;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Shared.Messages;

namespace EventDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var container = new BuiltinHandlerActivator())
            {
                var bus = Configure.With(container)
                    .Logging(l => l.ColoredConsole())
                    .Transport(t => t.UseAzureServiceBusAsOneWayClient(connectionStringNameOrConnectionString: Credentials.ServiceBusConnectionString))
                    .Routing(r => r.TypeBased().Map<SayHelloWorldCommand>("consumer-input"))
                    .Start();

                Console.WriteLine("Choose an option");
                Console.WriteLine("a) Send an a command");
                Console.WriteLine("b) Publish");
                var key = Console.ReadKey();
                if (key.KeyChar == 'a')
                {
                    bus.Advanced.SyncBus.Send(new SayHelloWorldCommand()
                    {
                        From = "Martin",
                    });
                }

                if (key.KeyChar == 'a')
                {
                    bus.Advanced.SyncBus.Publish(new HelloWorldPublishedEvent
                    {
                        From = "Poul"
                    });
                }
            }
        }
    }
}
