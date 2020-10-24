using System;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Microsoft.Extensions.Configuration;

namespace HubProducer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            var configuration = configurationBuilder
                      .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                      .AddUserSecrets(Assembly.GetExecutingAssembly(), false).Build();

            var name = configuration["eventhubname"];
            var cs = configuration["eventcs"];


            for (int j = 0; j < 10; j++)
            {
                await using (var client = new EventHubProducerClient(cs, name))
                {
                    using (var batch = await client.CreateBatchAsync())
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            batch.TryAdd(
                                new EventData(
                                    Encoding.UTF8.GetBytes($"card_{DateTime.Now.ToString()}_{DateTime.Now.Millisecond} ")));
                        }
                        await client.SendAsync(batch);
                    }
                }
                Thread.Sleep(100);
            }
            

            Console.WriteLine("Hello World!");
        }
    }
}
