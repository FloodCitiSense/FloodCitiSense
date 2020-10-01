using IIASA.FloodCitiSense.Mobile.Core.Extensions.MarkupExtensions;
using IIASA.FloodCitiSense.Mobile.Core.Interface;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace IIASA.FloodCitiSense.Views.Init
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrivacyPage : ContentPage, IXamarinView
    {
        public PrivacyPage()
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }
    }
}