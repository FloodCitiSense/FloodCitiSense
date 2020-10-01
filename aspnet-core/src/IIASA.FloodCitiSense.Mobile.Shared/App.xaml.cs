using Abp.Dependency;
using IIASA.FloodCitiSense.Helper;
using IIASA.FloodCitiSense.Mobile.Core.Core.Dependency;
using IIASA.FloodCitiSense.Mobile.Core.Extensions.MarkupExtensions;
using Microsoft.AppCenter;
using MonkeyCache.LiteDB;
using System;
using System.Threading.Tasks;
using IIASA.FloodCitiSense.Mobile.Core.Interface;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Device = Xamarin.Forms.Device;
[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace IIASA.FloodCitiSense
{
    public partial class App : Application, ISingletonDependency
    {
        private readonly bool _launchFromNotification;
        public static Action ExitApplication;

        public App(bool launchFromNotification)
        {
            _launchFromNotification = launchFromNotification;
            InitializeComponent();
            XF.Material.Forms.Material.Init(this);
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override async void OnStart()
        {
            base.OnStart();
            VersionTracking.Track();
            if (Device.RuntimePlatform == Device.iOS)
            {
                SetInitialScreenForIos();
            }
            Barrel.ApplicationId = "FloodCitiSense";

            AppCenter.Start("ios=780042de-7aab-4e67-915f-6e6171847c93;" +
                            "android=0a196250-f8bc-4e29-8615-b00a89fadcfd",
                typeof(Analytics), typeof(Crashes), typeof(App));
            AppCenter.LogLevel = LogLevel.Verbose;
            await InitNaviagation(_launchFromNotification);
            OnResume();
        }

        private static async Task InitNaviagation(bool launchFromNotification)
        {
            var nav = Resolver.Resolve<INavigationHelper>();
            await nav.InitNavigation(launchFromNotification);
        }

        private void SetInitialScreenForIos()
        {
            MainPage = new ContentPage
            {
                BackgroundColor = (Color)Current.Resources["PrimaryLight"],
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    Children =
                    {
                        new ActivityIndicator
                        {
                            IsRunning = true,
                            Color = Color.White
                        },
                        new Label
                        {
                            Text = "Initializing".Translate(),
                            TextColor = Color.White,
                            HorizontalTextAlignment = TextAlignment.Center,
                            VerticalTextAlignment = TextAlignment.Center
                        }
                    }
                }
            };
        }
    }
}