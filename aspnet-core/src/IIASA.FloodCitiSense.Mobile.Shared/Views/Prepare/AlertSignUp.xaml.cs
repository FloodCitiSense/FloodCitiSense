using IIASA.FloodCitiSense.Mobile.Core.Interface;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace IIASA.FloodCitiSense.Views.Prepare
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AlertSignUp : ContentPage, IXamarinView
    {
        public AlertSignUp()
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }
    }
}