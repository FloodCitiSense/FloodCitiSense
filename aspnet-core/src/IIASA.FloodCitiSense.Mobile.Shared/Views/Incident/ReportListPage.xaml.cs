using IIASA.FloodCitiSense.Mobile.Core.Interface;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IIASA.FloodCitiSense.Views.Incident
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReportListPage : ContentPage, IXamarinView
    {
        public ReportListPage()
        {
            InitializeComponent();
        }
    }
}