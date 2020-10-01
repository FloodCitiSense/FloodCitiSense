//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WalkthroughViewModel.cs" company="IIASA">
//    EOS
//  </copyright>
//  <summary>
//   WalkthroughViewModel.cs
//  </summary>
//  --------------------------------------------------------------------------------------------------------------------

using IIASA.FloodCitiSense.Mobile.Core.Base;
using IIASA.FloodCitiSense.Mobile.Core.Core.DataStorage;
using IIASA.FloodCitiSense.Mobile.Core.Services.Navigation;
using IIASA.FloodCitiSense.Views;
using IIASA.FloodCitiSense.Views.Init;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IIASA.FloodCitiSense.ViewModels.Walkthrough
{
    public class WalkthroughViewModel : XamarinViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IDataStorageManager _dataStorageManager;

        public WalkthroughViewModel(INavigationService navigationService, IDataStorageManager dataStorageManager)
        {
            _navigationService = navigationService;
            _dataStorageManager = dataStorageManager;
            this.GetStartedCommand = new Command(async () => await this.OnGetStartedClick());
        }

        private async Task OnGetStartedClick()
        {
            if (_dataStorageManager.HasKey(DataStorageKey.WelcomeViewed))
            {
                await _navigationService.SetMainPage<MasterPage>();
                await _navigationService.SetDetailPageAsync(typeof(MainPage));
            }
            else
            {
                await _dataStorageManager.StoreAsync(DataStorageKey.WelcomeViewed, true);
                await this._navigationService.SetMainPage<TermsPage>(null, true);
            }
        }

        public Command GetStartedCommand { get; set; }
    }
}