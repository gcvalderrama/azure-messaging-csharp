using System;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Microsoft.Extensions.Configuration;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using Azure.Messaging.EventHubs.Primitives;
using Azure.Storage.Blobs;

namespace HubConsumer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            var configuration = configurationBuilder
                      .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                      .AddUserSecrets(Assembly.GetExecutingAssembly(), false).Build();

            string blobcontainer = configuration["blobcontainer"];
            string blobcs = configuration["blobcs"];
            BlobContainerClient storageClient = new BlobContainerClient(blobcs, blobcontainer);

            //string consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;
            string consumerGroup = "democonsumer";

            string hubname = configuration["eventhubname"];
            string hubcs = configuration["eventcs"];

            var processor = new EventProcessorClient(storageClient, consumerGroup, hubcs,  hubname);
            processor.ProcessErrorAsync += async (ProcessErrorEventArgs ev) => {
                Console.WriteLine($"Partition {ev.PartitionId} : {ev.Exception.Message} ");

            };
            processor.ProcessEventAsync += async (ProcessEventArgs ev) => {
                var temp = Encoding.UTF8.GetString(ev.Data.Body.ToArray());
                Console.WriteLine($"Partition {ev.Partition} {ev.Data.SequenceNumber} : {temp}");
                await ev.UpdateCheckpointAsync(ev.CancellationToken);
            };

            await processor.StartProcessingAsync();

            await Task.Delay(TimeSpan.FromSeconds(60));

            await processor.StopProcessingAsync();
        }
    }
}
