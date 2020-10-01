using IIASA.FloodCitiSense.Mobile.Core.Interface;
using Xamarin.Forms;

namespace IIASA.FloodCitiSense.Views.Incident
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IncidentUploadPage : ContentPage, IXamarinView
    {
        public IncidentUploadPage()
        {
            InitializeComponent();
            Filter.SelectedIndex = 0;
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}