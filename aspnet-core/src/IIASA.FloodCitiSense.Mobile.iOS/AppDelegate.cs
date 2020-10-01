using FFImageLoading.Forms.Platform;
using Flurl.Http;
using Foundation;
using IIASA.FloodCitiSense.ApiClient;
using IIASA.FloodCitiSense.Mobile.Core.Core;
using IIASA.FloodCitiSense.Mobile.Core.Core.Exception;
using ImageCircle.Forms.Plugin.iOS;
using OxyPlot.Xamarin.Forms.Platform.iOS;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using WindowsAzure.Messaging;
using Acr.UserDialogs;
using IIASA.FloodCitiSense.Notification;
using Plugin.FacebookClient;
using Sharpnado.Presentation.Forms.iOS;
using UIKit;
using UserNotifications;
using Xamarin;
using Xamarin.Forms.Platform.iOS;
using XF.Material.iOS;
using CarouselViewRenderer = CarouselView.FormsPlugin.iOS.CarouselViewRenderer;
using Forms = Xamarin.Forms.Forms;
using CoreLocation;

namespace IIASA.FloodCitiSense
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to
    // application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : FormsApplicationDelegate
    {
        private const string ApsNotificationKey = "aps";
        private SBNotificationHub Hub { get; set; }
        private readonly CLLocationManager _locationManager = new CLLocationManager();

        //
        // This method is invoked when the application has loaded and is ready to run. In this
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;
            Forms.SetFlags("CollectionView_Experimental");
            ApplicationBootstrapper.InitializeIfNeeds<FloodCitiSenseXamarinIosModule>();
            SharpnadoInitializer.Initialize();
            Forms.Init();
            ImageCircleRenderer.Init();
            FormsGoogleMaps.Init("AIzaSyA_4G_We4nEDQ-R3mfipus-bIM1lHAMbUc");
            FormsGoogleMapsBindings.Init();
            CachedImageRenderer.Init();
            CarouselViewRenderer.Init();
            ConfigureFlurlHttp();
            PlotViewRenderer.Init();
            SetExitAction();
            Material.Init();
            Rg.Plugins.Popup.Popup.Init();
            if (options == null)
            {
                LoadApplication(new App(false));
            }
            else
            {
                LoadApplication(new App(options.ContainsKey(new NSString(ApsNotificationKey))));
            }

            FacebookClientManager.Initialize(app, options);

            ProcessNotification(options, true);


            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                _locationManager.RequestAlwaysAuthorization();
            }

            if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
            {
                _locationManager.AllowsBackgroundLocationUpdates = true;
            }

            RegisterForRemoteNotifications();

            return base.FinishedLaunching(app, options);
        }

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            return FacebookClientManager.OpenUrl(app, url, options);
        }

        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication,
            NSObject annotation)
        {
            return FacebookClientManager.OpenUrl(application, url, sourceApplication, annotation);
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            Debug.WriteLine($"FailedToRegisterForRemoteNotifications {error}");
        }

        public override async void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            Hub = new SBNotificationHub(PushNotificationConstants.ListenConnectionString,
                PushNotificationConstants.NotificationHubName);

            await PushNotificationHelper.UpdateCurrentCountryTag();

            // update registration with Azure Notification Hub
            Hub.UnregisterAll(deviceToken, (error) =>
            {
                if (error != null)
                {
                    Debug.WriteLine($"Unable to call unregister {error}");
                    return;
                }

                var tags = new NSSet(PushNotificationConstants.SubscriptionTags);
                Hub.RegisterNative(deviceToken, tags, (errorCallback) =>
                {
                    if (errorCallback != null)
                    {
                        Debug.WriteLine($"RegisterNativeAsync error: {errorCallback}");
                        UserDialogs.Instance.Toast("Error Registering to Push Notifications");
                    }
                });

                var templateExpiration = DateTime.Now.AddDays(120)
                    .ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
                Hub.RegisterTemplate(deviceToken, "defaultTemplate", PushNotificationConstants.APNTemplateBody,
                    templateExpiration, tags, (errorCallback) =>
                    {
                        if (errorCallback != null)
                        {
                            if (errorCallback != null)
                            {
                                Debug.WriteLine($"RegisterTemplateAsync error: {errorCallback}");
                                UserDialogs.Instance.Toast("Error Registering to Push Notifications template");
                            }
                        }
                    });
            });
        }

        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            ProcessNotification(userInfo, false);
        }

        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo,
            Action<UIBackgroundFetchResult> completionHandler)
        {
            ProcessNotification(userInfo, false);
        }

        void ProcessNotification(NSDictionary options, bool fromFinishedLaunching)
        {
            // make sure we have a payload
            if (options != null && options.ContainsKey(new NSString(ApsNotificationKey)))
            {
                // get the APS dictionary and extract message payload. Message JSON will be converted
                // into a NSDictionary so more complex payloads may require more processing
                NSDictionary aps = options.ObjectForKey(new NSString(ApsNotificationKey)) as NSDictionary;
                var message = GetValue(aps, "message");
                var messageTitle = GetValue(aps, "messageTitle");

                if (!string.IsNullOrWhiteSpace(message) && !string.IsNullOrWhiteSpace(messageTitle))
                {
                    UserDialogs.Instance.Alert(message, messageTitle);
                }
            }
            else
            {
                Debug.WriteLine($"Received request to process notification but there was no payload.");
            }
        }

        private static string GetValue(NSDictionary dictionary, string key)
        {
            string message = string.Empty;
            NSString nsString = new NSString(key);
            if (dictionary.ContainsKey(nsString))
            {
                message = dictionary[nsString].ToString();
            }

            Debug.WriteLine($"Received request to process notification. - sssss");
            return message;
        }

        private void RegisterForRemoteNotifications()
        {
            // register for remote notifications based on system version
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert |
                                                                      UNAuthorizationOptions.Sound |
                                                                      UNAuthorizationOptions.Sound,
                    (granted, error) =>
                    {
                        if (granted)
                            InvokeOnMainThread(UIApplication.SharedApplication.RegisterForRemoteNotifications);
                    });
            }
            else if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(
                    UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                    new NSSet());

                UIApplication.SharedApplication.RegisterUserNotificationSettings(pushSettings);
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            }
            else
            {
                UIRemoteNotificationType notificationTypes =
                    UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
                UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
            }
        }

        private static void TaskSchedulerOnUnobservedTaskException(object sender,
            UnobservedTaskExceptionEventArgs unobservedTaskExceptionEventArgs)
        {
            ExceptionHandler.LogException(unobservedTaskExceptionEventArgs.Exception);
            unobservedTaskExceptionEventArgs.SetObserved();
        }

        private static void CurrentDomainOnUnhandledException(object sender,
            UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            ExceptionHandler.LogException(unhandledExceptionEventArgs.ExceptionObject as Exception);
        }

        private static void SetExitAction()
        {
            App.ExitApplication = () => { Thread.CurrentThread.Abort(); };
        }

        private static void ConfigureFlurlHttp()
        {
            FlurlHttp.Configure(c => { c.HttpClientFactory = new ModernHttpClientFactory(); });
        }
    }
}