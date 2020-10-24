using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ProfileProducer
{
    public class DomainPublisher
    {
        public void execute(IConfiguration configuration) {
            string topicEndpoint = configuration["eventgriddomainurl"];
            string topicKey = configuration["eventgriddomainkey"];
            string topicHostname = new Uri(topicEndpoint).Host;
            TopicCredentials topicCredentials = new TopicCredentials(topicKey);
            EventGridClient client = new EventGridClient(topicCredentials);
            for (int i = 0; i < 10; i++)
            {
                client.PublishEventsAsync(topicHostname, GetEventsList()).GetAwaiter().GetResult();
                Console.Write("Published events to Event Grid topic.\n");
                Thread.Sleep(5000);
            }

        }
        static IList<EventGridEvent> GetEventsList()
        {
            List<EventGridEvent> eventsList = new List<EventGridEvent>();

            for (int i = 0; i < 1; i++)
            {
                eventsList.Add(new EventGridEvent()
                {
                    Id = Guid.NewGuid().ToString(),
                    EventType = "owlvey.Items.card",                    
                    Topic = $"profiledomain",
                    Data = new ProfileEventData()
                    {
                        Card = $"card #{i}"
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
