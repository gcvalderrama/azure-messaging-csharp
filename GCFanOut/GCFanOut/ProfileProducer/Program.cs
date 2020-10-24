
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Newtonsoft.Json;


namespace ProfileProducer
{
    class ProfileEventData
    {
        [JsonProperty(PropertyName = "card")]
        public string Card { get; set; }
    }
    class Program
    {   
        static void Main(string[] args)
        {
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            var configuration = configurationBuilder
                      .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)                      
                      .AddUserSecrets(Assembly.GetExecutingAssembly(), false).Build();

            var provider = new  TopicPublisher();
            provider.execute(configuration);
        }        
    }
}
