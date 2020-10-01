using IIASA.FloodCitiSense.Mobile.Core.Interface;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IIASA.FloodCitiSense.Views
{
    using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgotPasswordView : ContentPage, IXamarinView
    {
        public ForgotPasswordView()
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }
    }
}