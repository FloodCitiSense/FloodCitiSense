using IIASA.FloodCitiSense.Mobile.Core.Extensions.MarkupExtensions;
using IIASA.FloodCitiSense.Mobile.Core.Interface;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace IIASA.FloodCitiSense.Views.Init
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TermsPage : ContentPage, IXamarinView
    {
        public TermsPage()
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
            var htmlSource = new HtmlWebViewSource
            {
                Html = "TermsText".Translate()
            };
            WebView.Source = htmlSource;
        }
    }
}