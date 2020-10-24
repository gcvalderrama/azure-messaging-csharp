
using System;
using System.Net;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using Microsoft.Azure.NotificationHubs;
using System.Threading.Tasks;

namespace GCNotificationHub{
    public class NotificationHubMode {
        
        public static async Task Execute (IConfiguration configuration){
            var name = configuration["hub"];
            var cs = configuration["hub-cs"];
            NotificationHubClient hub = NotificationHubClient.CreateClientFromConnectionString(cs, name);
            string toast = @"<toast><visual><binding template=""ToastText01""><text id=""1"">AZ204</text></binding></visual></toast>";
            await hub.SendWindowsNativeNotificationAsync(toast);            
        }
    }
}
