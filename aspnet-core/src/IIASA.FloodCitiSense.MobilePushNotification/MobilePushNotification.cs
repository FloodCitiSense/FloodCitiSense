
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Extensions.Options;

namespace IIASA.FloodCitiSense.MobilePushNotification
{
    public interface IMobilePushNotification
    {
        Task Send(string message, string title, string country);
        Task Send(string message, string title, string[] countries);
    }

    public class MobilePushNotification : IMobilePushNotification
    {
        public MobilePushNotificationConfig MobilePushNotificationConfig { get; }

        public MobilePushNotification(IOptions<MobilePushNotificationConfig> mobilePushNotificationConfig)
        {
            MobilePushNotificationConfig = mobilePushNotificationConfig.Value;
        }

        public async Task Send(string message, string title, string country)
        {
            await SendTemplateNotificationsAsync(message, title, new[] {country});
        }

        public async Task Send(string message, string title, string[] countries)
        {
            await SendTemplateNotificationsAsync(message, title, countries);
        }

        private async Task SendTemplateNotificationsAsync(string message, string messageTitle, string[] tags)
        {
            NotificationHubClient hub = NotificationHubClient.CreateClientFromConnectionString(MobilePushNotificationConfig.FullAccessConnectionString, MobilePushNotificationConfig.NotificationHubName);
            Dictionary<string, string> templateParameters = new Dictionary<string, string>();
            
            // Send a template notification to each tag. This will go to any devices that
            // have subscribed to this tag with a template that includes "messageParam"
            // as a parameter
            foreach (var tag in tags)
            {
                templateParameters["messageParam"] = message;
                templateParameters["messageTitle"] = messageTitle;
                await hub.SendTemplateNotificationAsync(templateParameters, tag.ToLowerInvariant());
            }
        }
    }
}
