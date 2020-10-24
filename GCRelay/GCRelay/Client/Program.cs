using Microsoft.ServiceBus;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    [ServiceContract(Namespace = "https://owlveyrelay.servicebus.windows.net/owlveytcp")]
    public interface IWorkerService
    {
        [OperationContract]
        string Dowork(string message);
    }
    class Program
    {
        static void Main(string[] args)
        {
           

            Console.WriteLine("-------------------------------------------------------");
            
            string scheme = "sb";
            string serviceNamespace = "owlveyrelay";
            string servicePath = "owlveytcp";
            string policy = "relaypolicy";
            string accessKey = "bIQu0f0Su7YoTfwPdKxlvnH9Pk6omkc353ErvRpau7A=";

            ServiceBusEnvironment.SystemConnectivity.Mode = ConnectivityMode.AutoDetect;

            var cf = new ChannelFactory<IWorkerService>(new NetTcpRelayBinding() { 
                 IsDynamic = false
            }, new EndpointAddress(ServiceBusEnvironment.CreateServiceUri(scheme, serviceNamespace, servicePath)));

            cf.Endpoint.Behaviors.Add(new TransportClientEndpointBehavior { TokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(policy, accessKey) });

            var ch = cf.CreateChannel();
            string empty = "";
            while (string.IsNullOrWhiteSpace(empty)) {
                Console.WriteLine(ch.Dowork($"Cliente at {DateTime.Now.ToLongTimeString()}"));
                empty = Console.ReadLine();
            }            
            var t = (ICommunicationObject)ch;
            t.Close();
            



        }
    }
}
