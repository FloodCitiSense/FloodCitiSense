using IIASA.FloodCitiSense.Mobile.Core.Base;
using IIASA.FloodCitiSense.Mobile.Core.Core.DataStorage;
using IIASA.FloodCitiSense.Mobile.Core.Models.Common;
using IIASA.FloodCitiSense.Mobile.Core.Services.Navigation;
using IIASA.FloodCitiSense.Views;
using IIASA.FloodCitiSense.Views.Init;
using IIASA.FloodCitiSense.Views.Walkthrough;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace IIASA.FloodCitiSense.ViewModels.Init
{
    class WelcomePageModel : XamarinViewModel
    {
        private readonly IDataStorageManager dataStorageManager;

        private readonly INavigationService navigationService;

        private ObservableCollection<Carousel> _carouselItemSource;

        private View[] _views;

        public WelcomePageModel(IDataStorageManager dataStorageManager, INavigationService navigationService)
        {
            this.dataStorageManager = dataStorageManager;
            this.navigationService = navigationService;

            this.GetStartedCommand = new Command(this.OnGetStartedClick);
            this.NextCommand = new Command(async () => await this.OnNextClick());

        }

        public ICommand PageAppearingCommand => new Command(async () => await this.PageAppearingAsync());

        private async Task PageAppearingAsync()
        {

        }

        private async Task OnNextClick()
        {
            await this.navigationService.SetMainPage<WalkthroughView>(null, true);
        }

        public ObservableCollection<Carousel> CarouselItemSource
        {
            get => this._carouselItemSource;
            set
            {
                this._carouselItemSource = value;
                RaisePropertyChanged(() => CarouselItemSource);
            }
        }

        ObservableCollection<View> _myItemsSource;
        public ObservableCollection<View> MyItemsSource
        {
            set
            {
                _myItemsSource = value;
                RaisePropertyChanged(() => MyItemsSource);
            }
            get
            {
                return _myItemsSource;
            }
        }

        public Command GetStartedCommand { get; set; }
        public Command NextCommand { get; set; }

        public void GetCarouselList()
        {

        }

        public async void OnGetStartedClick()
        {
            //await this.navigationService.SetMainPage<TermsPage>(null, true);
            if (dataStorageManager.HasKey(DataStorageKey.WelcomeViewed))
            {
                await this.navigationService.SetMainPage<MainPage>(null, true);
            }
            else
            {
                await dataStorageManager.StoreAsync(DataStorageKey.WelcomeViewed, true);
                await this.navigationService.SetMainPage<TermsPage>(null, true);
            }
        }
    }
}
