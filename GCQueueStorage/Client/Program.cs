using System; // Namespace for Console output
using System.Configuration; // Namespace for ConfigurationManager
using System.Text;
using System.Threading.Tasks; // Namespace for Task
using Azure.Storage.Queues; // Namespace for Queue storage types
using Azure.Storage.Queues.Models; // Namespace for PeekedMessage



namespace Client
{
    class Program
    {
        public static void DequeueMessage()
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=owlvey;AccountKey=oJW4Nju0U2ayHoYaQuh67jujVDMVHtgMl4oU8WtnSelYI0tcVPdQIyW42oqo1ot0kKM5L5Q7tBmXo727tS9DbQ==;EndpointSuffix=core.windows.net";

            // Instantiate a QueueClient which will be used to create and manipulate the queue
            QueueClient queueClient = new QueueClient(connectionString, "samplequeue");

            if (queueClient.Exists())
            {
                // Get the next message
                QueueMessage[] retrievedMessage = queueClient.ReceiveMessages();

                // Process (i.e. print) the message in less than 30 seconds
                foreach (var item in retrievedMessage)
                {
                    Console.WriteLine($"Dequeued message: '{item.MessageText}'");
                    queueClient.DeleteMessage(item.MessageId, item.PopReceipt);
                }               
            }
        }
        public static void PeekMessage()
        {
            // Get the connection string from app settings
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=owlvey;AccountKey=oJW4Nju0U2ayHoYaQuh67jujVDMVHtgMl4oU8WtnSelYI0tcVPdQIyW42oqo1ot0kKM5L5Q7tBmXo727tS9DbQ==;EndpointSuffix=core.windows.net";

            // Instantiate a QueueClient which will be used to create and manipulate the queue
            QueueClient queueClient = new QueueClient(connectionString, "samplequeue");

            if (queueClient.Exists())
            {
                // Peek at the next message
                PeekedMessage[] peekedMessage = queueClient.PeekMessages();

                // Display the message
                Console.WriteLine($"Peeked message: '{peekedMessage[0].MessageText}'");
            }
        }
        static void Main(string[] args)
        {
            DequeueMessage();
            Console.WriteLine("Hello World!");
        }
    }
}
