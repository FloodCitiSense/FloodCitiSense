using CarouselView.FormsPlugin.Abstractions;
using IIASA.FloodCitiSense.Mobile.Core.Interface;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace IIASA.FloodCitiSense.Views.Walkthrough
{
    public partial class WalkthroughView : ContentPage, IXamarinView
    {
        private View[] _views;

        public WalkthroughView()
        {
            InitializeComponent();
            _views = new View[]
            {
                new SelectIntensity(),
                new TypeOfFlood(),
                new AddPictures(),
                new Visualize(),
                new ReportAndValidate(),
                //new Weather(),
                //new PrepareAndRespond(),
            };

            Carousel.ItemsSource = _views;
        }

        private void OnCarouselPositionSelected(object sender, PositionSelectedEventArgs e)
        {
            var currentView = _views[e.NewValue];

            var index = _views.IndexOf(currentView);

            GetStarted.IsVisible = _views.Length == (index + 1);

            Skip.IsVisible = _views.Length != (index + 1);
        }
    }
}