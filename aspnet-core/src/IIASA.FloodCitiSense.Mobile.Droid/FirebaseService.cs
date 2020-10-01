using System;
using System.Threading.Tasks;
using WindowsAzure.Messaging;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Util;
using Firebase.Messaging;
using IIASA.FloodCitiSense.Activities;
using IIASA.FloodCitiSense.Notification;

namespace IIASA.FloodCitiSense
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })] //[IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class FirebaseService : FirebaseMessagingService
    {
        const int NotificationId = 77777;
        private const string MessageTitle = "messageTitle";
        private const string Message = "message";

        public override void OnMessageReceived(RemoteMessage message)
        {
            Log.Info(PushNotificationConstants.DebugTag, $"OnMessageReceived- start");

            base.OnMessageReceived(message);
            string messageBody;
            string messageTitle = "Notification Received"; // need to translate

            if (message.GetNotification() != null)
            {
                messageBody = message.GetNotification().Body;
            }

            // NOTE: test messages sent via the Azure portal will be received here
            else
            {
                if (message.Data.ContainsKey(Message))
                {
                    messageBody = message.Data[Message];
                }
                else
                {
                    return;
                }

                if (message.Data.ContainsKey(MessageTitle))
                {
                    messageTitle = message.Data[MessageTitle];
                }
            }

            // convert the incoming message to a local notification
            SendLocalNotification(messageBody, messageTitle);
        }

        public override async void OnNewToken(string token)
        {
            // TODO: save token instance locally, or log if desired

            await SendRegistrationToServer(token);
        }

        void SendLocalNotification(string body, string title)
        {
            var intent = new Intent(this, typeof(SplashActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            intent.PutExtra(Message, body);
            intent.PutExtra(MessageTitle, title);
            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);

            var notificationBuilder = new NotificationCompat.Builder(this, PushNotificationConstants.NotificationChannelName)
                .SetContentTitle(title)
                .SetContentText(body)
                .SetAutoCancel(true)
                .SetSmallIcon(Resource.Drawable.icon)
                .SetShowWhen(false)
                .SetContentIntent(pendingIntent);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                notificationBuilder.SetChannelId(PushNotificationConstants.NotificationChannelName);
            }

            var notificationManager = NotificationManager.FromContext(this);
            notificationManager.Notify(NotificationId, notificationBuilder.Build());
        }

        private async Task SendRegistrationToServer(string token)
        {
            try
            {
                NotificationHub hub = new NotificationHub(PushNotificationConstants.NotificationHubName,
                    PushNotificationConstants.ListenConnectionString, this);

                // Get countryName for subscription tags
                await PushNotificationHelper.UpdateCurrentCountryTag();

                // register device with Azure Notification Hub using the token from FCM
                Registration registration = hub.Register(token, PushNotificationConstants.SubscriptionTags);

                // subscribe to the SubscriptionTags list with a simple template.
                string pnsHandle = registration.PNSHandle;
                TemplateRegistration templateReg = hub.RegisterTemplate(pnsHandle, "defaultTemplate",
                    PushNotificationConstants.FCMTemplateBody, PushNotificationConstants.SubscriptionTags);

            }
            catch (Exception e)
            {
                Log.Error(PushNotificationConstants.DebugTag, $"Error registering device: {e.Message}");
            }
        }
    }
}