using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ProfileProducer
{
    public class TopicPublisher
    {
        public void execute(IConfiguration configuration) {
            string topicEndpoint = configuration["eventgridurl"];

            string topicKey = configuration["eventgridkey"];
            Console.WriteLine($"send messages to {topicEndpoint}");

            string topicHostname = new Uri(topicEndpoint).Host;
            TopicCredentials topicCredentials = new TopicCredentials(topicKey);
            EventGridClient client = new EventGridClient(topicCredentials);
            for (int i = 0; i < 5; i++)
            {
                client.PublishEventsAsync(topicHostname, GetEventsList()).GetAwaiter().GetResult();
                Console.Write("Published events to Event Grid topic.\n");
                Thread.Sleep(5000);
            }
        }


        public IList<EventGridEvent> GetEventsList()
        {
            List<EventGridEvent> eventsList = new List<EventGridEvent>();

            for (int i = 0; i < 10; i++)
            {                
                eventsList.Add(new EventGridEvent()
                {
                    Id = Guid.NewGuid().ToString(),
                    EventType = "Items.card",                    
                    Data = new ProfileEventData()
                    {                        
                        Card = $"card #{DateTime.Now.ToLongTimeString()} {DateTime.Now.Millisecond}"
                    },
                    EventTime = DateTime.Now,
                    Subject = "Cards",
                    DataVersion = "2.0"
                });
            }

            return eventsList;
        }
    }
}
