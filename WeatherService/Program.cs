using Messages;
using Messages.NServiceBus.Events;
using Messages.ServiceBusRequest.CompanyDirectory.Requests;
using NServiceBus;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherService
{
    class Program
    {
        static void Main(string[] args)
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        static async Task AsyncMain()
        {
            Console.Title = "Weather Service";

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration("Weather Service");

            var scanner = endpointConfiguration.AssemblyScanner();
            scanner.ExcludeAssemblies("MySql.Data.dll");

            endpointConfiguration.EnableInstallers();
            endpointConfiguration.UseSerialization<JsonSerializer>();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.SendFailedMessagesTo("error");

            var transport = endpointConfiguration.UseTransport<MsmqTransport>();
            var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

            Debug.consoleMsg("Press Enter to exit.");

            /** PUT THIS BACK **/
            string entry;

            do
            {
                entry = Console.ReadLine();
            } while (!entry.Equals(""));
            /** END **/

            await endpointInstance.Stop().ConfigureAwait(false);
        }
    }
}
