using Microsoft.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    
    [ServiceContract(Namespace = "https://owlveyrelay.servicebus.windows.net/owlveytcp")]
    public interface IWorkerService
    {
        [OperationContract]
        string Dowork(string message);
    }
    
    public class Recharge : IWorkerService
    {
        public string Dowork(string message)
        {
            Console.WriteLine("message received  " + message);
            return "Message: " + message;
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            string scheme = "sb";
            string serviceNamespace = "owlveyrelay";
            string servicePath = "owlveytcp";
            string policy = "relaypolicy";
            string accessKey = "bIQu0f0Su7YoTfwPdKxlvnH9Pk6omkc353ErvRpau7A=";            
            Uri address = ServiceBusEnvironment.CreateServiceUri(scheme, serviceNamespace, servicePath);

            ServiceHost sh = new ServiceHost(typeof(Recharge), address);

            sh.AddServiceEndpoint(typeof(IWorkerService), new NetTcpRelayBinding() { 
                 IsDynamic = false
                }, address)
                .Behaviors.Add(new TransportClientEndpointBehavior
                {
                    TokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(policy, accessKey)
                });
            try
            {
                sh.Open();

                Console.WriteLine("Press ENTER to close");
                Console.ReadLine();                
            }
            catch (Exception)
            {
                throw;
            }
            finally {
                sh.Close();
            }
            
        }

    }
}
