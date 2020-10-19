// https://docs.microsoft.com/en-us/previous-versions/windows/apps/hh868252(v=win.10)

using System;
using System.Net;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace GCNotificationHub
{
    class Program
    {        
        static void Main(string[] args)
        {
            ConfigurationBuilder builder = new ConfigurationBuilder(); 
            var configuration = builder.AddJsonFile("appsettings.json")
                                            .AddUserSecrets(Assembly.GetExecutingAssembly(), false)
                                            .AddEnvironmentVariables()
                                            .Build();

            //ManualNotification.Exceute(configuration);
            NotificationHubMode.Execute(configuration).GetAwaiter().GetResult();
            //The uri parameter is the channel Uniform Resource Identifier (URI) requested by the app and passed to the cloud server
        
        }

    }
}
