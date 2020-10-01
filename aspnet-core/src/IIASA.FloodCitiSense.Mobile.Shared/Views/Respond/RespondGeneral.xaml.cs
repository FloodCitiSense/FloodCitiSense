using IIASA.FloodCitiSense.Mobile.Core.Interface;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace IIASA.FloodCitiSense.Views.Respond
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RespondGeneral : ContentPage, IXamarinView
    {
        public RespondGeneral()
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }
    }
}