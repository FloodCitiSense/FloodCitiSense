using IIASA.FloodCitiSense.Mobile.Core.Interface;
using IIASA.FloodCitiSense.ViewModels.Weather;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace IIASA.FloodCitiSense.Views.Weather
{
    public partial class WeatherPage : ContentPage, IXamarinView
    {
        public WeatherPage()
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);


            var vm = BindingContext as WeatherPageModel;

            vm?.ElementService.RenderElements(Layout);
        }
    }
}