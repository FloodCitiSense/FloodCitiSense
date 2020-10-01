using IIASA.FloodCitiSense.Mobile.Core.Interface;
using Xamarin.Forms;

namespace IIASA.FloodCitiSense.Views
{
    using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

    public partial class _SampleView : ContentPage, IXamarinView
    {
        public _SampleView()
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }
    }
}