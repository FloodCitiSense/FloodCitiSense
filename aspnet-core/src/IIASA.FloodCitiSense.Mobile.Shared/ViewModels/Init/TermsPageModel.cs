using Acr.UserDialogs;
using IIASA.FloodCitiSense.Mobile.Core.Base;
using IIASA.FloodCitiSense.Mobile.Core.Core.DataStorage;
using IIASA.FloodCitiSense.Mobile.Core.Extensions.MarkupExtensions;
using IIASA.FloodCitiSense.Mobile.Core.Services.Navigation;
using IIASA.FloodCitiSense.Views;
using IIASA.FloodCitiSense.Views.Account;
using Xamarin.Forms;

namespace IIASA.FloodCitiSense.ViewModels.Init
{
    class TermsPageModel : XamarinViewModel
    {
        private readonly IDataStorageManager _dataStorageManager;

        private readonly INavigationService _navigationService;

        public Command ProceedCommand { get; set; }

        public TermsPageModel(IDataStorageManager dataStorageManager, INavigationService navigationService)
        {
            this._dataStorageManager = dataStorageManager;
            this._navigationService = navigationService;
            this.ProceedCommand = new Command(OnProceedClickAsync);
            this.IsNotAccepted = !dataStorageManager.HasKey(DataStorageKey.TermsAccepted);
        }

        public bool IsTermsAccepted { get; set; }

        public bool IsNotAccepted { get; set; }

        private async void OnProceedClickAsync()
        {
            if (IsTermsAccepted)
            {
                await _dataStorageManager.StoreAsync(DataStorageKey.TermsAccepted, true);
                await _navigationService.SetMainPage<MasterPage>();
                await _navigationService.SetDetailPageAsync(typeof(LoginView));
            }
            else
            {
                await UserDialogs.Instance.AlertAsync("TermsNotAccepted".Translate(), "AcceptTermsMessage".Translate(), "Ok".Translate());
            }
        }
    }
}
