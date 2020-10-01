using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Auth.Api;
using Android.OS;
using Android.Runtime;
using Android.Views;
using FFImageLoading.Forms.Platform;
using IIASA.FloodCitiSense.Mobile.Core.Core.Exception;
using ImageCircle.Forms.Plugin.Droid;
using Java.Security;
using OxyPlot.Xamarin.Forms.Platform.Android;
using Plugin.CurrentActivity;
using Plugin.FacebookClient;
using Plugin.Permissions;
using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Sharpnado.Presentation.Forms.Droid;
using Xamarin;
using Xamarin.Forms.GoogleMaps.Android;
using Xamarin.Forms.Platform.Android;
using Forms = Xamarin.Forms.Forms;
using Platform = Xamarin.Essentials.Platform;

namespace IIASA.FloodCitiSense
{
    [Activity(
        Label = "FloodCitiSense",
        Icon = "@drawable/icon",
        MainLauncher = false,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.Window.RequestFeature(WindowFeatures.ActionBar);
            // Name of the MainActivity theme you had there before.
            // Or you can use global::Android.Resource.Style.ThemeHoloLight
            base.SetTheme(Resource.Style.MainTheme);

            base.OnCreate(bundle);

            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;
            SharpnadoInitializer.Initialize();
            Forms.Init(this, bundle);
            ImageCircleRenderer.Init();
            CachedImageRenderer.Init(true);
            CarouselView.FormsPlugin.Android.CarouselViewRenderer.Init();
            Platform.Init(this, bundle);
            CrossCurrentActivity.Current.Init(this, bundle);
            PlotViewRenderer.Init();
            GoogleService.Init(this);
            XF.Material.Droid.Material.Init(this, bundle);
            FacebookClientManager.Initialize(this);
            Rg.Plugins.Popup.Popup.Init(this, bundle);
            var platformConfig = new PlatformConfig
            {
                BitmapDescriptorFactory = new CachingNativeBitmapDescriptorFactory()
            };

            FormsGoogleMaps.Init(this, bundle, platformConfig);
            FormsGoogleMapsBindings.Init();
            LoadApplication(new App(string.IsNullOrEmpty(Intent.GetStringExtra("message")) == false));
            HandleNotification();
        }

        private void HandleNotification()
        {
            string message = Intent.GetStringExtra("message");
            string messageTitle = Intent.GetStringExtra("messageTitle");
            if (string.IsNullOrWhiteSpace(message) || string.IsNullOrWhiteSpace(messageTitle))
            {
                return;
            }
            UserDialogs.Instance.Alert(message,messageTitle);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions,
            [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private static void PrintSignature()
        {
            try
            {
                PackageInfo info = Android.App.Application.Context.PackageManager.GetPackageInfo(Android.App.Application.Context.PackageName, PackageInfoFlags.Signatures);
                foreach (var signature in info.Signatures)
                {
                    MessageDigest md = MessageDigest.GetInstance("SHA");
                    md.Update(signature.ToByteArray());
                    System.Diagnostics.Debug.WriteLine("---------------------Digest---------------------------");
                    System.Diagnostics.Debug.WriteLine(Convert.ToBase64String(md.Digest()));
                }
            }
            catch (NoSuchAlgorithmException e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }

        public override void OnBackPressed()
        {
            if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
            {
                // Do something if there are some pages in the `PopupStack`
            }
            else
            {
                // Do something if there are not any pages in the `PopupStack`
            }
        }


        /// <summary>
        ///     Called when [activity result].
        /// </summary>
        /// <param name="requestCode">The request code.</param>
        /// <param name="resultCode">The result code.</param>
        /// <param name="data">The data.</param>
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            FacebookClientManager.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == 1)
            {
                var result = Auth.GoogleSignInApi.GetSignInResultFromIntent(data);
                GoogleService.Instance.OnAuthCompleted(result);
            }

            if (FacebookService.Instance != null)
                FacebookService.Instance.CallbackManager.OnActivityResult(requestCode, (int)resultCode, data);
        }

        private static void TaskSchedulerOnUnobservedTaskException(object sender,
            UnobservedTaskExceptionEventArgs unobservedTaskExceptionEventArgs)
        {
            Console.WriteLine(unobservedTaskExceptionEventArgs.Exception);
            ExceptionHandler.LogException(unobservedTaskExceptionEventArgs.Exception);
            unobservedTaskExceptionEventArgs.SetObserved();
        }

        private static void CurrentDomainOnUnhandledException(object sender,
            UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            Console.WriteLine(unhandledExceptionEventArgs.ExceptionObject);
            ExceptionHandler.LogException(unhandledExceptionEventArgs.ExceptionObject as Exception);
        }
    }
}