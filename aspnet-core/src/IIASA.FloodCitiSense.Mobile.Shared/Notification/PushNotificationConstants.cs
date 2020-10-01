namespace IIASA.FloodCitiSense.Notification
{
    public static class PushNotificationConstants
    {
        /// <summary>
        /// Notification channels are used on Android devices starting with "Oreo"
        /// </summary>
        public static string NotificationChannelName { get; set; } = "FloodCitiSenseChannel";

        /// <summary>
        /// This is the name of your Azure Notification Hub, found in your Azure portal.
        /// </summary>
        public static string NotificationHubName { get; set; } = "floodcitisense";

        /// <summary>
        /// This is the "DefaultListenSharedAccessSignature" connection string, which is
        /// found in your Azure Notification Hub portal under "Access Policies".
        /// 
        /// You should always use the ListenShared connection string. Do not use the
        /// FullShared connection string in a client application.
        /// </summary>
        public static string ListenConnectionString { get; set; } = "Endpoint=sb://floodcitisense.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=oz+yeoB0zr2QRHzQrwjCw6x9ZxSBi1Ns9HHfMw0H1PQ=";

        /// <summary>
        /// Tag used in log messages to easily filter the device log
        /// during development.
        /// </summary>
        public static string DebugTag { get; set; } = "FloodCitiSenseNotify";

        /// <summary>
        /// The tags the device will subscribe to. These could be subjects like
        /// news, sports, and weather. Or they can be tags that identify a user
        /// across devices. Here we are using device location country name.
        /// </summary>
        public static string[] SubscriptionTags { get; set; } = { "default" };

        public static bool UserInputComplete { get; set; } = false;

        /// <summary>
        /// This is the template json that Android devices will use. Templates
        /// are defined by the device and can include multiple parameters.
        /// </summary>
        public static string FCMTemplateBody { get; set; } = "{\"data\":{\"message\":\"$(messageParam)\",\"messageTitle\":\"$(messageTitle)\"}}";

        /// <summary>
        /// This is the template json that Apple devices will use. Templates
        /// are defined by the device and can include multiple parameters.
        /// alert text is internally used by ios to show in the notification pop up when app is closed. Do not delete this.
        /// </summary>
        public static string APNTemplateBody { get; set; } = "{\"aps\":{\"alert\":\"$(messageTitle)\",\"message\":\"$(messageParam)\",\"messageTitle\":\"$(messageTitle)\"}}";
    }
}