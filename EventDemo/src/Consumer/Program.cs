using System;
using System.Threading.Tasks;
using Rebus.Activation;
using Rebus.Config;
using Shared.Messages;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var container = new BuiltinHandlerActivator())
            {
                container.Handle<SayHelloWorldCommand>((bus, context, msg) =>
                {
                    Console.WriteLine($"Retrieved {msg.GetType().Namespace}... Say Hello From {msg.From}");
                    return Task.CompletedTask;;
                });

                Configure.With(container)
                    .Logging(l => l.ColoredConsole())
                    .Transport(t => t.UseAzureServiceBus(connectionStringNameOrConnectionString: Credentials.ServiceBusConnectionString, "consumer-input"))
                    .Start();

                Console.ReadLine();
            }
        }
    }
}