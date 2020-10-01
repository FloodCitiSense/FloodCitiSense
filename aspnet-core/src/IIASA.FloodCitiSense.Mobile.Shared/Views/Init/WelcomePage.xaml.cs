using CarouselView.FormsPlugin.Abstractions;
using IIASA.FloodCitiSense.Mobile.Core.Interface;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace IIASA.FloodCitiSense.Views.Init
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WelcomePage : ContentPage, IXamarinView
    {
        public WelcomePage()
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }

        private void OnCarouselPositionSelected(object sender, PositionSelectedEventArgs e)
        {

        }
    }
}