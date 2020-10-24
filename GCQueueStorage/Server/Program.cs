using System; // Namespace for Console output
using System.Configuration; // Namespace for ConfigurationManager
using System.Text;
using System.Threading.Tasks; // Namespace for Task
using Azure.Storage.Queues; // Namespace for Queue storage types
using Azure.Storage.Queues.Models; // Namespace for PeekedMessage


namespace Server
{
    class Program
    {
      
        public static void InsertMessage()
        {
            // Get the connection string from app settings
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=owlvey;AccountKey=oJW4Nju0U2ayHoYaQuh67jujVDMVHtgMl4oU8WtnSelYI0tcVPdQIyW42oqo1ot0kKM5L5Q7tBmXo727tS9DbQ==;EndpointSuffix=core.windows.net";

            // Instantiate a QueueClient which will be used to create and manipulate the queue
            QueueClient queueClient = new QueueClient(connectionString, "samplequeue");

            for (int i = 0; i < 10; i++)
            {                
                queueClient.SendMessage("message " + i.ToString());
            }                  
            

            Console.WriteLine($"Inserted: completed");
        }
        static void Main(string[] args)
        {
            InsertMessage();
        }
    }
}
