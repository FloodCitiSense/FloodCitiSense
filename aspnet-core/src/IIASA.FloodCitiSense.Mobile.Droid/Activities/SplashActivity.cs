using System;
using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;
using Android.Util;
using Flurl.Http;
using IIASA.FloodCitiSense.ApiClient;
using IIASA.FloodCitiSense.Mobile.Core.Core;
using IIASA.FloodCitiSense.Notification;

namespace IIASA.FloodCitiSense.Activities
{
    [Activity(Theme = "@style/MyTheme.Splash", Label = "FloodCitiSense",
        MainLauncher = true,
        NoHistory = false,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        LaunchMode = LaunchMode.SingleTop)]
    public class SplashActivity : AppCompatActivity
    {
        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();
            ApplicationBootstrapper.InitializeIfNeeds<FloodCitiSenseXamarinAndroidModule>();
            StartApplication();
        }

        /// <summary>
        ///  Performing some startup work that takes a bit of time
        /// </summary>
        private void StartApplication()
        {
            var intent = new Intent(this, typeof(MainActivity));
            if (Intent.Extras != null)
            {
                intent.PutExtras(Intent.Extras); // copy push info from splash to main
            }

            StartActivity(intent);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Splash);
            UserDialogs.Init(this);
            ConfigureFlurlHttp();
            CreateNotificationChannel();
        }

        private static void ConfigureFlurlHttp()
        {
            FlurlHttp.Configure(c =>
            {
                c.HttpClientFactory = new ModernHttpClientFactory();
            });
        }

        /// <summary>
        /// Called when the app is in the background and a notification is clicked on (also called each time the app is minimized and the brought back up), a new <c>Intent</c> is created
        ///     and sent out, since we use <c>LaunchMode</c> set to <c>SingleTop</c> this method is called instead of the app being restarted.
        /// </summary>
        /// <param name="intent">The <c>Intent</c> that was set when the call was made. If started from a notification click, extra <c>message</c> values can be extracted.</param>
        protected override void OnNewIntent(Intent intent)
        {
            if (intent.Extras != null)
            {
                Intent.PutExtras(intent.Extras);
            }
            base.OnNewIntent(intent);
        }

        void CreateNotificationChannel()
        {
            // Notification channels are new as of "Oreo".
            // There is no need to create a notification channel on older versions of Android.
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channelName = PushNotificationConstants.NotificationChannelName;
                var channelDescription = String.Empty;
                var channel = new NotificationChannel(channelName, channelName, NotificationImportance.Default)
                {
                    Description = channelDescription
                };

                var notificationManager = (NotificationManager)GetSystemService(NotificationService);
                notificationManager.CreateNotificationChannel(channel);
                Log.Info(PushNotificationConstants.DebugTag, $"channel created -SplashActivity");
            }
        }

    }
}