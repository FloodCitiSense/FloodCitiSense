namespace IIASA.FloodCitiSense.MobilePushNotification
{
    public interface IMobilePushNotificationConfig
    {
        /// <summary>
        /// This is the name of your Azure Notification Hub, found in your Azure portal.
        /// </summary>
        string NotificationHubName { get; set; }

        /// <summary>
        /// This is the "DefaultFullSharedAccessSignature" connection string, which is
        /// found in your Azure Notification Hub portal under "Access Policies".
        /// </summary>
        string FullAccessConnectionString { get; set; }
    }

    public class MobilePushNotificationConfig : IMobilePushNotificationConfig
    {
        /// <summary>
        /// This is the name of your Azure Notification Hub, found in your Azure portal.
        /// </summary>
        public string NotificationHubName { get; set; } = "floodcitisense";

        /// <summary>
        /// This is the "DefaultFullSharedAccessSignature" connection string, which is
        /// found in your Azure Notification Hub portal under "Access Policies".
        /// </summary>
        public string FullAccessConnectionString { get; set; } = "Endpoint=sb://floodcitisense.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=f8/WzgYgn4DUYJTI1KdNkQIRPSvyjF1vPWFFroAvVA0=";

    }
}