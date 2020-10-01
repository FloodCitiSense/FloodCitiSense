using IIASA.FloodCitiSense.Mobile.Core.Interface;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace IIASA.FloodCitiSense.Views.Prepare
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrepareGeneral : ContentPage, IXamarinView
    {
        public PrepareGeneral()
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }
    }
}