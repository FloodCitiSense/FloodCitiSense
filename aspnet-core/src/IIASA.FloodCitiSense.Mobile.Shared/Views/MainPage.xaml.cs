
using IIASA.FloodCitiSense.Mobile.Core.Interface;
using Xamarin.Forms;

namespace IIASA.FloodCitiSense.Views
{
    using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage, IXamarinView
    {
        public MainPage()
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }
    }
}